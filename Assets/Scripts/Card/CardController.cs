using UnityEngine;

namespace Card
{
    public class CardController
    {
        private Transform _cardTransform;
        private SpriteRenderer _cardRenderer;
        
        public CardController(Transform cardObjectTransform, SpriteRenderer spriteRenderer)
        {
            _cardTransform = cardObjectTransform;
            _cardRenderer = spriteRenderer;
        }

        public void DoUpdate()
        {
            
        }
    }
}