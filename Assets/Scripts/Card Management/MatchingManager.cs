using System.Collections;
using System.Collections.Generic;
using Card;
using UnityEngine;

public class MatchingManager
{
    private List<CardController> _cards = new();
    private CardInfoSO _cardInfoSo;
    
    public MatchingManager(List<CardController> generatedCards, CardInfoSO cardInfo)
    {
        if (generatedCards is not { Count: > 0 })
            Debug.LogError("MatchManager constructed without any cards");
        if (cardInfo == null)
            Debug.LogError("MatchManager constructed without any cardInfo");
        
        _cards = generatedCards;
        _cardInfoSo = cardInfo;
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
            break;
        }
    }
}
