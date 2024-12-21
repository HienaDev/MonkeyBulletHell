using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject newGameMessage;
    [SerializeField] private GameObject exitMessage;
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        if (!SaveManager.Instance.SaveFileExists())
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
    }
    
    public void ContinueGame()
    {
        if (SaveManager.Instance.SaveFileExists())
        {
            SaveManager.Instance.SetTargetScene(sceneToLoad);

            SceneManager.sceneLoaded += SaveManager.Instance.OnGameSceneLoaded;

            SceneManager.LoadScene(sceneToLoad);
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

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void ConfirmNewGame()
    {
        SaveManager.Instance.DeleteSaveFile();

        SaveManager.Instance.SetTargetScene(sceneToLoad);

        SceneManager.sceneLoaded += SaveManager.Instance.OnGameSceneLoaded;        

        SceneManager.LoadScene(sceneToLoad);
    }

    public void CancelNewGame()
    {
        newGameMessage.SetActive(false);
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
}
