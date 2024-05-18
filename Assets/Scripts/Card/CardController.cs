using UnityEngine;

namespace Card
{
    public class CardController
    {
        public Transform _cardTransform;
        private SpriteRenderer _cardRenderer;

        private Sprite _backGraphic;
        private Sprite _frontGraphic;

        public CardController(Transform cardObjectTransform, SpriteRenderer spriteRenderer, Vector2 cardSize)
        {
            _cardTransform = cardObjectTransform;
            _cardRenderer = spriteRenderer;
            
            _cardRenderer.drawMode = SpriteDrawMode.Sliced;
            _cardRenderer.size = cardSize;
            _cardTransform.localScale = Vector3.one;
        }

        public void DoUpdate()
        {
            
        }

        public void SetupCardInfo(Sprite cardGraphicBack, Sprite cardGraphicFront)
        {
            var cardDimension = _cardRenderer.size;
            _backGraphic = cardGraphicBack;
            _frontGraphic = cardGraphicFront;

            _cardRenderer.sprite = _backGraphic;
            _cardRenderer.size = cardDimension;
        }

        public bool IsMouseOverCard(Vector3 mouseWorldPos)
        {
            var cardBounds = _cardRenderer.bounds;
            return cardBounds.Contains(mouseWorldPos); //TODO: Change this to a custom detection check as Contains is an Injected call
        }
    }
}