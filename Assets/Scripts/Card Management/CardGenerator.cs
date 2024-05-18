using System.Collections.Generic;
using Card;
using UnityEngine;

namespace Card_Management
{
    public class CardGenerator
    {

        public CardGenerator(int rows, int columns, CardInfoSO cardInfo)
        {
            _rows = rows;
            _columns = columns;
            _cardDimensions = cardInfo.cardDimensions;
            _cardPadding = cardInfo.cardPadding;
            _cardInfoSo = cardInfo;
        }
        
        private int _rows = 4;
        private int _columns = 3;

        private Vector2 _cardDimensions;
        private Vector2 _cardPadding;
        private CardInfoSO _cardInfoSo;

        public List<CardController> StartGeneration()
        {
            var generatedCards = new List<CardController>();
    
            var xPos = _cardDimensions.x / 2 + _cardPadding.x;
            var yPos = -(_cardDimensions.y / 2 + _cardPadding.y);
        
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var newYPos = yPos - (_cardDimensions.y + _cardPadding.y) * y;
                    generatedCards.Add(GenerateCard(xPos, newYPos));
                }
                
                xPos += _cardDimensions.x + _cardPadding.x;
            }

            return generatedCards;
        }

        private CardController GenerateCard(float xPos, float yPos)
        {
            //Setup The Visual/GameObject aspect of the cards
            var cardObject = new GameObject($"Card {xPos}, {yPos}");
            var spriteRenderer = cardObject.AddComponent<SpriteRenderer>();
            var cardObjectTransform = cardObject.GetComponent<Transform>();
            cardObjectTransform.position = new Vector3(xPos, yPos, 0);
            
            //Setup card logic class, we decouple this from Monobehaviour so we can control the updates and lifetime
            var newCardController = new CardController(cardObjectTransform, spriteRenderer, _cardDimensions);
            
            return newCardController;
        }
    }
}
