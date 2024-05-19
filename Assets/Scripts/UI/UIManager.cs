using Card_Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
    
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
        
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverScoreText;
    
        [SerializeField] private GameObject pausePanel;

        private void Awake()
        {
            GameManager.OnScoreUpdate += UpdateScore;
            GameManager.OnGameCompleted += ShowGameOverPanel;
            GameManager.OnGameRestarted += GameRestarted;
            if (comboText != null) comboText.text = "";
        }

        private void GameRestarted()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.OnScoreUpdate -= UpdateScore;
            GameManager.OnGameCompleted -= ShowGameOverPanel;
            GameManager.OnGameRestarted -= GameRestarted;
        }

        private void ShowGameOverPanel(int scoreCount)
        {
            if (gameOverPanel == null) return;
            
            gameOverPanel.SetActive(true);
            gameOverScoreText.text = $"Score: {scoreCount}";
        }

        private void UpdateScore(int currentScore, int comboCount)
        {
            if (scoreText != null) scoreText.text = $"Score: {currentScore}";
            if (comboText != null) comboText.text = comboCount > 1 ? $"Combo: x{comboCount}" : "";
        }

        public void ShowPauseMenu()
        {
            pausePanel.SetActive(true);
            GameManager.Instance.PauseGame(true);
        }
        
        public void ClosePauseMenu()
        {
            pausePanel.SetActive(false);
            GameManager.Instance.PauseGame(false);

        }
    }
}
