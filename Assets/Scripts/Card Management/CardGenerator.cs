using System.Collections.Generic;
using Card;
using UnityEngine;

namespace Card_Management
{
    public class CardGenerator
    {
        private int _row = 4;
        private int _columns = 3;

        private readonly Vector2 _cardDimensions = new Vector2(2, 5);
        private readonly Vector2 _cardPadding = new Vector2(.5f, .5f);

        private List<CardController> StartGeneration(int row, int _columns, CardInfoSO cardInfo)
        {
            var generatedCards = new List<CardController>();
    
            var xPos = 0f;
            var yPos = 0f;
        
            for (int x = 0; x < row; x++)
            {
                xPos += (_cardDimensions.x) + _cardPadding.x;

                generatedCards.Add(GenerateCard(xPos, yPos));
            
                for (int y = 0; y < _columns; y++)
                {
                    var newYPos = yPos - _cardDimensions.y - _cardPadding.y;
                    GenerateCard(xPos, newYPos);
                }
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
            cardObjectTransform.localScale = _cardDimensions;
            
            //Setup card logic class, we decouple this from Monobehaviour so we can control the updates and lifetime
            var newCardController = new CardController(cardObjectTransform, spriteRenderer);
            
            return newCardController;
        }
    }
}
