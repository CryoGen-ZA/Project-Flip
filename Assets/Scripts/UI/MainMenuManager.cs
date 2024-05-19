using System.Linq;
using Card;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuManager : MonoBehaviour
{
    public CardInfoSO[] cardInfo;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject quitButton;

    [Header("Layout Options")] 
    [SerializeField] private GameObject LayoutMenu;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private TMP_InputField rowInput;
    [SerializeField] private TMP_InputField columnInput;
    [SerializeField] private TextMeshProUGUI errorWarning;

    private const string GameplayScene = "GamePlay Scene";
    
    public void OnNewGameSelected(Vector2Int layout)
    {
        PlayerData.MatchData = null;
        PlayerData.Layout = layout;
        var randomIndex = Random.Range(0, cardInfo.Length);
        StartNewGame(cardInfo[randomIndex]);
    }

    public void OnContinueSelected()
    {
       PlayerDataSerializer.Load();
       
       var storedTheme = PlayerData.CurrentActiveTheme = cardInfo.FirstOrDefault(t => t.themeName == PlayerData.MatchData.cardInfoID);
       if (storedTheme == null)
           storedTheme = cardInfo[0];
       
       StartNewGame(storedTheme);
    }

    private void StartNewGame(CardInfoSO cardInfo)
    {
        PlayerData.CurrentActiveTheme = cardInfo;
        SceneManager.LoadScene(GameplayScene);
    }

    private void Awake()
    {
        continueButton.interactable = PlayerDataSerializer.PlayerHasData();
    }

    public void DisplayLayoutField()
    {
        LayoutMenu.SetActive(true);
        MainMenu.SetActive(false);
        quitButton.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        quitButton.SetActive(true);
        LayoutMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ChoosePredefinedLayout(int option)
    {
        switch (option)
        {
            case 0:
                OnNewGameSelected(new Vector2Int(2,2));
                break;
            case 1:
                OnNewGameSelected(new Vector2Int(5,2));
                break;
            case 2:
                OnNewGameSelected(new Vector2Int(6,5));
                break;
        }
    }

    public void CustomLayoutChosen()
    {
        //we do validation check on the character types so we dont need to tryParse
        if (rowInput.characterValidation != TMP_InputField.CharacterValidation.Integer || 
            columnInput.characterValidation != TMP_InputField.CharacterValidation.Integer) return;
        
        var rows = int.Parse(rowInput.text);
        var columns = int.Parse(columnInput.text);

        if (rows > 0 && columns > 0 && IsEven(rows * columns))
            OnNewGameSelected(new Vector2Int(columns, rows));
        else
            errorWarning.text =
                $"Oh oh, {rows} x {columns} results in an uneven amount of cells, please try a different combination";
        
    }

    public void QuitApplication() => Application.Quit();
    private bool IsEven(int totalCards) => totalCards % 2 == 0;
}
