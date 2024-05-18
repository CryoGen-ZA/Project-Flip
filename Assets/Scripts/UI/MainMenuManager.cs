using System.Collections;
using System.Collections.Generic;
using Card;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public CardInfoSO cardInfo;
    
    public void OnNewGameSelected()
    {
        StartNewGame(cardInfo);
    }

    private void StartNewGame(CardInfoSO cardInfo)
    {
        PlayerData.CurrentActiveTheme = cardInfo;
        SceneManager.LoadScene("GamePlay Scene");
    }
}
