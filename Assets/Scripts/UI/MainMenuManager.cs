using System;
using System.Linq;
using Card;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour
{
    public CardInfoSO[] cardInfo;
    [SerializeField] private Button continueButton;
    
    public void OnNewGameSelected()
    {
        PlayerData.MatchData = null;
        var randomIndex = Random.Range(0, cardInfo.Length);
        StartNewGame(cardInfo[randomIndex]);
    }

    public void OnContinueSelected()
    {
       PlayerDataSerializer.Load();
       
       var storedTheme = PlayerData.CurrentActiveTheme = cardInfo.FirstOrDefault(t => t.themeName == PlayerData.MatchData.cardInfoID);
       if (storedTheme == null)
           storedTheme = cardInfo[0];

       PlayerData.CurrentActiveTheme = storedTheme;
       
       StartNewGame(storedTheme);
    }

    private void StartNewGame(CardInfoSO cardInfo)
    {
        PlayerData.CurrentActiveTheme = cardInfo;
        SceneManager.LoadScene("GamePlay Scene");
    }

    private void Awake()
    {
        continueButton.interactable = PlayerDataSerializer.PlayerHasData();
    }
}
