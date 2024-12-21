using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string saveFileName;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Chest chest;
    [SerializeField] private Tutorial tutorial;

    public static SaveManager Instance { get; private set; }
    private string targetScene;

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
    }

    private struct GameSaveData
    {
        public PlayerInventory.SaveData playerInventoryData;
        public Chest.SaveData chestData;
        public Tutorial.SaveData tutorialData;
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

        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveFileName, jsonSaveData);

        print("Game Saved");
    }

    public void CreateNewGame()
    {
        LookForReferences();

        GameSaveData saveData;

        saveData.playerInventoryData = playerInventory.GetSaveData();
        saveData.chestData = chest.GetSaveData();
        saveData.tutorialData = tutorial.GetSaveData();

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
}