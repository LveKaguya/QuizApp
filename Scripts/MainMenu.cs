using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; 

public class MainMenu : MonoBehaviour
{
    public GameObject nicknamePanel;
    public TMP_InputField nicknameInput;
    public Button submitNicknameButton;
    public Button programmingConceptsButton;
    public Button algorithmsButton;
    public Button dataStructuresButton;
    public TextMeshProUGUI titleText;
    public Button exitButton;

    private string playerNickname;

    private readonly Color[] modeColors = new Color[]
    {
        new Color(0f, 120f/255f, 1f), 
        new Color(0f, 1f, 0f),       
        new Color(1f, 0f, 0f)         
    };

    void Start()
    {
        // Ensure nickname panel is active at start
        if (nicknamePanel != null)
            nicknamePanel.SetActive(true);

        // Disable mode selection buttons at start
        if (programmingConceptsButton != null)
        {
            programmingConceptsButton.gameObject.SetActive(false);
            programmingConceptsButton.GetComponent<Image>().color = modeColors[0]; // Blue
        }
        if (algorithmsButton != null)
        {
            algorithmsButton.gameObject.SetActive(false);
            algorithmsButton.GetComponent<Image>().color = modeColors[1]; // Green
        }
        if (dataStructuresButton != null)
        {
            dataStructuresButton.gameObject.SetActive(false);
            dataStructuresButton.GetComponent<Image>().color = modeColors[2]; // Red
        }

        // Ensure title text is visible
        if (titleText != null)
            titleText.gameObject.SetActive(true);

        // Ensure exit button is visible
        if (exitButton != null)
            exitButton.gameObject.SetActive(true);

        // Set up the submit button listener
        if (submitNicknameButton != null)
        {
            submitNicknameButton.onClick.AddListener(OnSubmitNickname);
        }

        // Set up the exit button listener
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExit);
        }

        // Focus the nickname input field
        StartCoroutine(FocusInputField());
    }

    private IEnumerator FocusInputField()
    {
        yield return new WaitForEndOfFrame();
        if (nicknameInput != null)
        {
            nicknameInput.ActivateInputField();
            nicknameInput.Select();
        }
    }

    void OnSubmitNickname()
    {
        if (nicknameInput == null || string.IsNullOrWhiteSpace(nicknameInput.text))
        {
            Debug.LogWarning("Nickname cannot be empty!");
            return;
        }

        playerNickname = nicknameInput.text.Trim();
        PlayerPrefs.SetString("PlayerNickname", playerNickname);
        PlayerPrefs.Save();

        // Hide the nickname panel
        if (nicknamePanel != null)
            nicknamePanel.SetActive(false);

        // Enable the mode selection buttons
        if (programmingConceptsButton != null)
            programmingConceptsButton.gameObject.SetActive(true);
        if (algorithmsButton != null)
            algorithmsButton.gameObject.SetActive(true);
        if (dataStructuresButton != null)
            dataStructuresButton.gameObject.SetActive(true);
    }

    void OnExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void StartGameMode(int mode)
    {
        PlayerPrefs.SetInt("GameMode", mode);
        PlayerPrefs.Save();
        SceneManager.LoadScene("QuizGame");
    }
}