using System.Collections.Generic;
using Card;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Card_Management
{
    public class MatchingManager
    {
        private List<CardController> _cards = new();
        private List<CardController> _activeCards = new();
        private readonly CardInfoSO _cardInfoSo;

        private CardController _firstCard;
        private CardController _secondCard;

        private int _currentScore;
        private int _currentCombo;
        private int _currentMatchCount;


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
            
                if (card.isMatched && card.IsDoneAnimating() && card.isFlipped)
                    _activeCards.Remove(card);
            
                else if (card.IsDoneAnimating() && !card.isFlipped) 
                    _activeCards.Remove(card);
            
                else if (card.IsDoneAnimating() && card != _firstCard && card != _secondCard)
                    card.FlipCard();
            
                card.DoUpdate();
            }
        }

        public void SetupCards()
        {
            _currentMatchCount = 0;
            _currentScore = 0;
            _currentCombo = 0;
            
            //Create ID list
            var ids = new List<int>();

            for (var i = 0; i < _cards.Count / 2; i++)
            {
                var idNumber = Random.Range(0, _cardInfoSo.cardIcons.Length);
                ids.Add(idNumber);
                ids.Add(idNumber);
            }

            ids = ShuffleList(ids);

            for (var index = 0; index < _cards.Count; index++)
            {
                var card = _cards[index];
                var cardIcon = _cardInfoSo.cardIcons[ids[index]];
                var id = ids[index];
                card.SetupCardInfo(_cardInfoSo.cardGraphicBack, _cardInfoSo.cardGraphicFront, cardIcon, id,
                    _cardInfoSo.flipTime);
            }
        }

        private List<int> ShuffleList(List<int> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var t = Random.Range(0, i + 1);
                (list[t], list[i]) = (list[i], list[t]);
            }

            return list;
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
            if (_activeCards.Contains(card) || card.isMatched) return;
        
            if (_firstCard != null && _secondCard != null)
            {
                _firstCard = card;
                _secondCard = null;
            }
            else if (_firstCard == null)
            {
                _firstCard = card;
            }
            else 
            {
                _secondCard = card;
                ConfirmMatch();
            }

            card.FlipCard();
            GameManager.Instance.PlaySFX(_cardInfoSo.flipSfx);
            _activeCards.Add(card);
        }

        private void ConfirmMatch()
        {
            if (_firstCard == null || _secondCard == null) return;

            if (_firstCard.cardId != _secondCard.cardId)
            {
                _currentCombo = 0;
                GameManager.Instance.PlaySFX(_cardInfoSo.matchIncorrectSfx);
                return;
            }
        
            _firstCard.SetMatched();
            _secondCard.SetMatched();
            _currentMatchCount += 2;

            _currentScore += 2 + 2 * _currentCombo;
            _currentCombo++;

            GameManager.Instance.ScoreUpdate(_currentScore, _currentCombo);
            GameManager.Instance.PlaySFX(_cardInfoSo.matchConfirmedSfx);
            
            if (_currentMatchCount == _cards.Count)
            {
                GameManager.Instance.FireGameCompleted(_currentScore);
                GameManager.Instance.PlaySFX(_cardInfoSo.completedSfx);
            }
        }

        public void RestorePreviousMatch(List<int> matchDataMatchedCards, int matchDataScore, int matchDataComboMultiplier)
        {
            foreach (var cardIndex in matchDataMatchedCards)
            {
                var card = _cards[cardIndex];
                card.FlipCard();
                card.SetMatched();
                _activeCards.Add(card);
            }

            _currentScore = matchDataScore;
            _currentCombo = matchDataComboMultiplier;
            _currentMatchCount = matchDataMatchedCards.Count;
            GameManager.Instance.ScoreUpdate(_currentScore, _currentCombo);
        }

        public int GetCurrentScore() => _currentScore;

        public int GetCurrentCombo() => _currentCombo;

        public List<int> GetMatchedCards()
        {
            //Readability Could be improved by Linq expressions but opted not to as it comes with a performance cost on mobile
            var matchCardsList = new List<int>();
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i].isMatched)
                    matchCardsList.Add(i);
            }
            return matchCardsList;
        }
    }
}
