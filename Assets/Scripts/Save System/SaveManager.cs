using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    [Header("Save File")]
    [SerializeField] private string saveFileName;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Chest chest;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private StoryTelling story;

    [Header("Save Notification")]
    [SerializeField] private TextMeshProUGUI saveNotificationText;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float displayDuration = 2.0f;

    private Coroutine currentSaveNotificationCoroutine;
    private string targetScene;

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        saveFileName = Application.persistentDataPath + "/" + saveFileName;

        LookForReferences();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LookForReferences()
    {
        if (playerInventory == null)
        {
            playerInventory = FindFirstObjectByType<PlayerInventory>();
        }

        if (chest == null)
        {
            chest = FindFirstObjectByType<Chest>();
        }

        if (tutorial == null)
        {
            tutorial = FindFirstObjectByType<Tutorial>();
        }

        if (story == null)
        {
            story = FindFirstObjectByType<StoryTelling>();
        }

        if (saveNotificationText == null)
        {
            GameObject notificationObject = GameObject.Find("Save Game Text");
            if (notificationObject != null)
            {
                saveNotificationText = notificationObject.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private struct GameSaveData
    {
        public PlayerInventory.SaveData playerInventoryData;
        public Chest.SaveData chestData;
        public Tutorial.SaveData tutorialData;
        public StoryTelling.SaveData storyData;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            QuickSaveGame();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            QuickLoadGame();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    public void QuickSaveGame()
    {
        LookForReferences();

        GameSaveData saveData;

        saveData.playerInventoryData = playerInventory.GetSaveData();
        saveData.chestData = chest.GetSaveData();
        saveData.tutorialData = tutorial.GetSaveData();
        saveData.storyData = story.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveFileName, jsonSaveData);

        if (saveNotificationText != null && currentSaveNotificationCoroutine == null)
        {
            currentSaveNotificationCoroutine = StartCoroutine(ShowSaveNotification("Game Saved"));
        }
    }

    public void CreateNewGame()
    {
        LookForReferences();

        GameSaveData saveData;

        saveData.playerInventoryData = playerInventory.GetSaveData();
        saveData.chestData = chest.GetSaveData();
        saveData.tutorialData = tutorial.GetSaveData();

        story.LoadSaveData(new StoryTelling.SaveData { isNewGame = true });
        saveData.storyData = story.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveFileName, jsonSaveData);

        print("Game Created");
    }

    public bool QuickLoadGame()
    {
        LookForReferences();

        if (File.Exists(saveFileName))
        {
            string jsonSaveData = File.ReadAllText(saveFileName);

            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonSaveData);

            playerInventory.LoadSaveData(saveData.playerInventoryData);
            chest.LoadSaveData(saveData.chestData);
            tutorial.LoadSaveData(saveData.tutorialData);
            story.LoadSaveData(saveData.storyData);

            print("Game Loaded");

            return true;
        }
        else
        {
            print("No save file found");

            return false;
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFileName))
        {
            File.Delete(saveFileName);

            print("Save file deleted");
        }
        else
        {
            print("No save file found");
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(saveFileName);
    }

    public void SetTargetScene(string sceneName)
    {
        targetScene = sceneName;
    }

    public void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetScene)
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;

            StartCoroutine(LoadGameDataAfterDelay());
        }
    }

    public IEnumerator LoadGameDataAfterDelay()
    {
        yield return null;

        LookForReferences();

        if (SaveFileExists())
        {
            QuickLoadGame();
        }
        else
        {
            CreateNewGame();
        }
    }

    private IEnumerator ShowSaveNotification(string message)
    {
        if (saveNotificationText == null)
            yield break;

        saveNotificationText.text = message;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            saveNotificationText.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        saveNotificationText.alpha = 1;

        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            saveNotificationText.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        saveNotificationText.alpha = 0;

        currentSaveNotificationCoroutine = null;
    }
}