using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject newGameMessage;
    [SerializeField] private GameObject exitMessage;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;


    private void Start()
    {
        if (!SaveManager.Instance.SaveFileExists())
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (newGameMessage.activeSelf)
            {
                CancelNewGame();
            }
            else if (settingsMenu.activeSelf)
            {
                CloseSettingsMenu();
            }
            else if (creditsMenu.activeSelf)
            {
                CloseCreditsMenu();
            }
            else if (exitMessage.activeSelf)
            {
                CancelExitGame();
            }
            else
            {
                ExitGame();
            }
        }
    }
    
    public void ContinueGame()
    {
        if (SaveManager.Instance.SaveFileExists())
        {
            SaveManager.Instance.SetTargetScene(sceneToLoad);

            SceneManager.sceneLoaded += SaveManager.Instance.OnGameSceneLoaded;

            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
    }

    public void NewGame()
    {
        if (SaveManager.Instance.SaveFileExists())
        {
            newGameMessage.SetActive(true);
        }
        else
        {   
            SaveManager.Instance.SetTargetScene(sceneToLoad);

            SceneManager.sceneLoaded += SaveManager.Instance.OnGameSceneLoaded;

            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
    }

    public void ConfirmNewGame()
    {
        SaveManager.Instance.DeleteSaveFile();

        SaveManager.Instance.SetTargetScene(sceneToLoad);

        SceneManager.sceneLoaded += SaveManager.Instance.OnGameSceneLoaded;

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    public void CancelNewGame()
    {
        newGameMessage.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenCreditsMenu()
    {
        creditsMenu.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        exitMessage.SetActive(true);
    }

    public void ConfirmExitGame()
    {
#if UNITY_STANDALONE
        // Exit application if running standalone
        Application.Quit();
#endif
#if UNITY_EDITOR
        // Stop game if running in editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CancelExitGame()
    {
        exitMessage.SetActive(false);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        if (loadingText != null) loadingText.text = "Loading... 0%";

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f) * 100;

            if (loadingText != null) loadingText.text = $"Loading... {Mathf.RoundToInt(progress)}%";

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}
