using UnityEngine;

namespace Card
{
    public class CardController
    {
        public Transform _cardTransform;
        private SpriteRenderer _cardRenderer;
        private SpriteRenderer _cardIconRenderer;

        private Sprite _backGraphic;
        private Sprite _frontGraphic;
        private Sprite _cardIcon;
        private int _cardId;
        
        private bool _isFlipped;
        private bool _animating;
        private float _flipSpeed;
        private float _currentAnimatingTime;
        private bool _invertAnimation;
        private bool _match;

        public int cardId => _cardId;

        public CardController(Transform cardObjectTransform, SpriteRenderer spriteRenderer, Vector2 cardSize)
        {
            _cardTransform = cardObjectTransform;
            _cardRenderer = spriteRenderer;
            
            _cardRenderer.drawMode = SpriteDrawMode.Sliced;
            _cardRenderer.size = cardSize;
            _cardTransform.localScale = Vector3.one;
        }

        public bool isFlipped => _isFlipped;
        public bool isMatched => _match;

        public void DoUpdate()
        {
            if (!_animating) return;
            
            if (!_invertAnimation)
            {
                if (!AnimateCard(false)) return;
                
                FlipVisuals();
                _invertAnimation = true;
            }
            else if (_invertAnimation)
            {
                if (!AnimateCard(true)) return;
                    
                _animating = false;
                _invertAnimation = false;
            }
        }

        private bool AnimateCard(bool invert)
        {
            var normalizedTime = _currentAnimatingTime / (_flipSpeed / 2);
            
            var targetScale = invert ? Vector3.one : Vector3.up;
            var currentScale = invert ? Vector3.up : Vector3.one;
            
            _cardTransform.localScale = Vector3.Lerp(currentScale, targetScale, normalizedTime);

            _currentAnimatingTime += Time.deltaTime;

            if (normalizedTime < 1) return false;
            
            _currentAnimatingTime = 0;
            return true;
        }

        private void FlipVisuals()
        {
            _isFlipped = !_isFlipped;
            _cardRenderer.sprite = _isFlipped ? _frontGraphic : _backGraphic;
            _cardIconRenderer.gameObject.SetActive(_isFlipped);
        }

        public void SetupCardInfo(Sprite cardGraphicBack, Sprite cardGraphicFront, Sprite cardIcon, int cardId, float flipSpeed)
        {
            var cardDimension = _cardRenderer.size;
            _backGraphic = cardGraphicBack;
            _frontGraphic = cardGraphicFront;

            _cardRenderer.sprite = _backGraphic;
            _cardRenderer.size = cardDimension;

            _cardIcon = cardIcon;
            _cardId = cardId;
            _flipSpeed = flipSpeed;

            _isFlipped = false;
            _match = false;

            SetupCardIcon();
        }

        private void SetupCardIcon()
        {
            if (_cardIconRenderer == null)
            {
                var cardIconObject = new GameObject($"Card Icon {_cardId}");
                cardIconObject.transform.SetParent(this._cardTransform);
                _cardIconRenderer = cardIconObject.AddComponent<SpriteRenderer>();
                cardIconObject.transform.position = _cardTransform.position + -Vector3.forward * 0.1f;
            }
            _cardIconRenderer.sprite = _cardIcon;
            _cardIconRenderer.size = new Vector2(_cardRenderer.size.x / 2, _cardRenderer.size.y / 2);
            _cardIconRenderer.gameObject.SetActive(false);
        }

        public bool IsMouseOverCard(Vector3 mouseWorldPos)
        {
            var cardBounds = _cardRenderer.bounds;
            return cardBounds.Contains(mouseWorldPos); //TODO: Change this to a custom detection check as Contains is an Injected call
        }

        public void FlipCard() => _animating = true;

        public bool IsDoneAnimating() => _animating == false;

        public void SetMatched()
        {
            _match = true;
        }
    }
}