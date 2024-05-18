using System.Collections;
using System.Collections.Generic;
using Card;
using UnityEngine;

public class MatchingManager
{
    private List<CardController> _cards = new();
    private CardInfoSO _cardInfoSo;
    
    private CardController _firstCard;
    private CardController _secondCard;

    private List<CardController> _activeCards = new();
    
    public MatchingManager(List<CardController> generatedCards, CardInfoSO cardInfo)
    {
        if (generatedCards is not { Count: > 0 })
            Debug.LogError("MatchManager constructed without any cards");
        if (cardInfo == null)
            Debug.LogError("MatchManager constructed without any cardInfo");
        
        _cards = generatedCards;
        _cardInfoSo = cardInfo;
    }

    public void DoUpdate()
    {
        for (var index = _activeCards.Count - 1; index >= 0; index--)
        {
            var card = _activeCards[index];
            
            if (card.IsDoneAnimating() && !card.isFlipped) _activeCards.Remove(card);
            
            else if (card.IsDoneAnimating() && card != _firstCard && card != _secondCard)
                card.FlipCard();
            
            card.DoUpdate();
        }
    }

    public void SetupCards()
    {
        foreach (var card in _cards)
        {
            card.SetupCardInfo(_cardInfoSo.cardGraphicBack, _cardInfoSo.cardGraphicFront, _cardInfoSo.cardIcons[0], 0, _cardInfoSo.flipTime);
        }
    }

    public void CheckForCardClick(Vector3 mouseWorldPos)
    {
        foreach (var card in _cards)
        {
            if (!card.IsMouseOverCard(mouseWorldPos)) continue;
            AssignCard(card);
            break;
        }
    }

    private void AssignCard(CardController card)
    {
        if (_activeCards.Contains(card)) return;
        
        if (_firstCard != null && _secondCard != null)
        {
            _firstCard = card;
            _secondCard = null;
        }
        else
        {
            if (_firstCard == null)
            {
                _firstCard = card;
            }
            else if (_secondCard == null)
            {
                _secondCard = card;
            }
        }

        card.FlipCard();
        _activeCards.Add(card);
    }
}
