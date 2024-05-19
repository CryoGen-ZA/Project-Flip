using System;
using Card;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Card_Management
{
    public class GameManager : MonoBehaviour
    {
        private MatchingManager _matchingManager;
        private Camera _cam;
        private AudioSource _audioSource;
        
        public int _rows;
        public int _columns;
        public CardInfoSO cardInfo;
        public MatchingManager MatchManager => _matchingManager;

        public static GameManager Instance { get; private set; }
        public static event Action<int, int> OnScoreUpdate;
        public static event Action<int> OnGameCompleted;
        public static event Action OnGameRestarted;

        private int currentSeed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            _audioSource = GetComponent<AudioSource>();

            if (PlayerData.MatchData != null)
            {
                _rows = PlayerData.MatchData.layoutInfo.y;
                _columns = PlayerData.MatchData.layoutInfo.x;
                currentSeed = PlayerData.CurrentSeed;
            }
            else
            {
                currentSeed = (int)DateTime.Now.Ticks;
                Random.InitState(currentSeed);
            }
            cardInfo = PlayerData.CurrentActiveTheme;
        }

        private void Update()
        {
            _matchingManager?.DoUpdate();
        }

        private void Start()
        {
            var cardGenerator = new CardGenerator(_rows, _columns, cardInfo);
            var generatedCards = cardGenerator.StartGeneration();
            
            _matchingManager = new MatchingManager(generatedCards, cardInfo);
            
            if (_matchingManager != null)
                _matchingManager.SetupCards();

            if (PlayerData.MatchData != null)
                _matchingManager.RestorePreviousMatch(PlayerData.MatchData.matchedCards, PlayerData.MatchData.score, PlayerData.MatchData.comboMultiplier);
            
            _cam = Camera.main;
            FitCardLayoutToCameraView(_rows, _columns, cardInfo);
        }
        
        public void ResetGame()
        {
            currentSeed = (int)DateTime.Now.Ticks;
            Random.InitState(currentSeed);
            _matchingManager.SetupCards();
            OnGameRestarted?.Invoke();
        }

        public void LeaveGame()
        {
            SceneManager.LoadScene("Menu Scene");
        }

        public void SaveGame()
        {
            var playerMatchData = new PlayerMatchData();

            playerMatchData.layoutInfo = new Vector2Int(_columns, _rows);
            playerMatchData.score = _matchingManager.GetCurrentScore();
            playerMatchData.comboMultiplier = _matchingManager.GetcurrentCombo();
            playerMatchData.seedkey = currentSeed;
            playerMatchData.cardInfoID = cardInfo.themeName;
            playerMatchData.matchedCards = _matchingManager.GetMatchedCards();

            PlayerData.MatchData = playerMatchData;
            PlayerDataSerializer.Save();
        }

        private void FitCardLayoutToCameraView(int rows, int columns, CardInfoSO cardInfoSo)
        {
            var totalHeight = rows * (cardInfoSo.cardDimensions.y + cardInfoSo.cardPadding.y);
            var totalWidth = columns * (cardInfoSo.cardDimensions.x + cardInfoSo.cardPadding.x);

            totalWidth += cardInfo.cardPadding.x;
            totalHeight += cardInfo.cardPadding.y;

            var centrePoint = new Vector3(totalWidth / 2f, -totalHeight / 2f, -10);

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

        public void ScoreUpdate(int currentScore, int currentCombo)
        {
            OnScoreUpdate?.Invoke(currentScore, currentCombo);
        }
        
        public void FireGameCompleted(int currentScore)
        {
            OnGameCompleted?.Invoke(currentScore);
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || _audioSource == null) return;
            
            _audioSource.PlayOneShot(clip);
        }
    }
}
