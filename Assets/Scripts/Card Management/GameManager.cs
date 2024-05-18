using Card;
using UnityEngine;

namespace Card_Management
{
    public class GameManager : MonoBehaviour
    {
        private MatchingManager _matchingManager;
        private Camera _cam;
        
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

            _cam = Camera.main;
            FitCardLayoutToCameraView(_rows, _columns, cardInfo);
        }

        private void FitCardLayoutToCameraView(int rows, int columns, CardInfoSO cardInfoSo)
        {
            var totalHeight = rows * (cardInfoSo.cardDimensions.y + cardInfoSo.cardPadding.y);
            var totalWidth = columns * (cardInfoSo.cardDimensions.x + cardInfoSo.cardPadding.x);

            totalWidth += cardInfo.cardPadding.x;
            totalHeight += cardInfo.cardPadding.y;

            var centrePoint = new Vector3(totalWidth / 2f, -totalHeight / 2f, -1);

            _cam.transform.position = centrePoint;
            
            ScaleCameraViewToCardLayout(totalWidth, totalHeight);
        }

        private void ScaleCameraViewToCardLayout(float totalWidth, float totalHeight)
        {
            var cameraHeight = _cam.orthographicSize * 2;
            var cameraWidth = cameraHeight * _cam.aspect;

            var layoutWidthRatio = totalWidth / cameraWidth;
            var layoutHeightRatio = totalHeight / cameraHeight;
            
            _cam.orthographicSize = layoutWidthRatio < layoutHeightRatio ? 
                totalHeight / 2 : 
                totalWidth / 2 / _cam.aspect;
        }
    }
}
