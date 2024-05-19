using System;
using Card;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Input;
using Random = UnityEngine.Random;

namespace Card_Management
{
    public class GameManager : MonoBehaviour
    {
        private MatchingManager _matchingManager;
        private Camera _cam;
        private AudioSource _audioSource;
        public MatchingManager MatchManager => _matchingManager;

        public static GameManager Instance { get; private set; }
        public static event Action<int, int> OnScoreUpdate;
        public static event Action<int> OnGameCompleted;
        public static event Action OnGameRestarted;

        private int _currentSeed;
        private bool _paused;
        private int _rows;
        private int _columns;
        private CardInfoSO _cardInfo;
        private DeviceOrientation _currentOrientation;


        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            _audioSource = GetComponent<AudioSource>();

            SetupMatchData();
        }

        private void SetupMatchData()
        {
            if (PlayerData.MatchData != null)
            {
                _rows = PlayerData.MatchData.layoutInfo.y;
                _columns = PlayerData.MatchData.layoutInfo.x;
                _currentSeed = PlayerData.MatchData.seedKey;
            }
            else
            {
                _currentSeed = (int)DateTime.Now.Ticks;
                _rows = PlayerData.Layout.y;
                _columns = PlayerData.Layout.x;
            }
            
            Random.InitState(_currentSeed);
            _cardInfo = PlayerData.CurrentActiveTheme;
        }

        private void Start()
        {
            var cardGenerator = new CardGenerator(_rows, _columns, _cardInfo);
            var generatedCards = cardGenerator.StartGeneration();
            
            _matchingManager = new MatchingManager(generatedCards, _cardInfo);
            _matchingManager?.SetupCards();

            if (PlayerData.MatchData != null && _matchingManager != null)
                _matchingManager.RestorePreviousMatch(PlayerData.MatchData.matchedCards, PlayerData.MatchData.score, PlayerData.MatchData.comboMultiplier);
            
            _cam = Camera.main;
            FitCardLayoutToCameraView(_rows, _columns, _cardInfo);
        }
        
        private void Update()
        {
            _matchingManager?.DoUpdate();
            CheckForDeviceOrientation();
        }

        private void CheckForDeviceOrientation()
        {
            if (deviceOrientation == _currentOrientation) return;
            _currentOrientation = deviceOrientation;
            FitCardLayoutToCameraView(_rows, _columns, _cardInfo);
        }

        public void ResetGame()
        {
            _currentSeed = (int)DateTime.Now.Ticks;
            Random.InitState(_currentSeed);
            _matchingManager.SetupCards();
            OnGameRestarted?.Invoke();
        }

        public void LeaveGame()
        {
            SceneManager.LoadScene("Menu Scene");
        }

        public void QuitAndSave()
        {
            SaveGame();
            LeaveGame();
        }

        private void SaveGame()
        {
            var playerMatchData = new PlayerMatchData
            {
                layoutInfo = new Vector2Int(_columns, _rows),
                score = _matchingManager.GetCurrentScore(),
                comboMultiplier = _matchingManager.GetCurrentCombo(),
                seedKey = _currentSeed,
                cardInfoID = _cardInfo.themeName,
                matchedCards = _matchingManager.GetMatchedCards()
            };

            PlayerData.MatchData = playerMatchData;
            PlayerDataSerializer.Save();
        }

        private void FitCardLayoutToCameraView(int rows, int columns, CardInfoSO cardInfoSo)
        {
            var totalHeight = rows * (cardInfoSo.cardDimensions.y + cardInfoSo.cardPadding.y);
            var totalWidth = columns * (cardInfoSo.cardDimensions.x + cardInfoSo.cardPadding.x);

            totalWidth += _cardInfo.cardPadding.x;
            totalHeight += _cardInfo.cardPadding.y;

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

        public void ScoreUpdate(int currentScore, int currentCombo) => 
            OnScoreUpdate?.Invoke(currentScore, currentCombo);
        
        public void FireGameCompleted(int currentScore) =>
            OnGameCompleted?.Invoke(currentScore);

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || _audioSource == null) return;
            
            _audioSource.PlayOneShot(clip);
        }

        public void PauseGame(bool pause) => _paused = pause;

        public bool IsGamePaused() => _paused;
    }
}
