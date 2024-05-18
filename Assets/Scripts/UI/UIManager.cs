using Card_Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
    
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
    
        private void Awake()
        {
            GameManager.OnScoreUpdate += UpdateScore;
            if (comboText != null) comboText.text = "";
        }

        private void UpdateScore(int currentScore, int comboCount)
        {
            if (scoreText != null) scoreText.text = $"Score: {currentScore}";
            if (comboText != null) comboText.text = comboCount > 1 ? $"Combo: x{comboCount}" : "";
        }
    }
}
