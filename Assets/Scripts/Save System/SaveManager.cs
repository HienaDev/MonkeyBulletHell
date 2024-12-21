using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string saveFileName;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Chest chest;

    public static SaveManager Instance { get; private set; }

    private void Start()
    {
        saveFileName = Application.persistentDataPath + "/" + saveFileName;

        SceneManager.sceneLoaded += OnSceneLoaded;

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

    private struct GameSaveData
    {
        public PlayerInventory.SaveData playerInventoryData;
        public Chest.SaveData chestData;
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
        GameSaveData saveData;

        saveData.playerInventoryData = playerInventory.GetSaveData();
        saveData.chestData = chest.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveFileName, jsonSaveData);

        print("Game Saved");
    }

    public void CreateNewGame()
    {
        GameSaveData saveData;

        saveData.playerInventoryData = playerInventory.GetSaveData();
        saveData.chestData = chest.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(saveFileName, jsonSaveData);

        print("Game Created");
    }

    public bool QuickLoadGame()
    {
        if (File.Exists(saveFileName))
        {
            string jsonSaveData = File.ReadAllText(saveFileName);

            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonSaveData);

            playerInventory.LoadSaveData(saveData.playerInventoryData);
            chest.LoadSaveData(saveData.chestData);

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
}
