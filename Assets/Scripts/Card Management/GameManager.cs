using Card;
using UnityEngine;

namespace Card_Management
{
    public class GameManager : MonoBehaviour
    {
        private MatchingManager _matchingManager;
        public int _rows;
        public int _columns;
        public CardInfoSO cardInfo;

        private void Start()
        {
            var cardGenerator = new CardGenerator(_rows, _columns, cardInfo);
            var generatedCards = cardGenerator.StartGeneration();
            
            _matchingManager = new MatchingManager(generatedCards, cardInfo);
            
            if (_matchingManager != null)
                _matchingManager.SetupCards();
        }
    }
}
