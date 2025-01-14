using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] storyTiles;

    private GameObject currentTile;
    private int currentTileIndex;
    private bool isNewGame;

    void Awake()
    {
        // Carrega o estado de isNewGame corretamente antes do Start
        if (SaveManager.Instance != null && SaveManager.Instance.SaveFileExists())
        {
            SaveManager.Instance.QuickLoadGame();
        }
        else
        {
            isNewGame = true;
        }
    }

    void Start()
    {
        if (isNewGame)
        {
            background.SetActive(true);

            Time.timeScale = 0f;
            currentTileIndex = 0;
            currentTile = storyTiles[currentTileIndex];

            currentTile.SetActive(true);
        }
    }

    public void NextTile()
    {
        if (!isNewGame) return;

        currentTileIndex++;
        currentTile.SetActive(false);

        if (currentTileIndex >= storyTiles.Length)
        {
            Time.timeScale = 1f;
            background.SetActive(false);
            isNewGame = false;
        }
        else
        {
            currentTile = storyTiles[currentTileIndex];
            currentTile.SetActive(true);
        }
    }

    void Update()
    {
        Debug.Log("isNewGame: " + isNewGame);
    }

    [System.Serializable]
    public struct SaveData
    {
        public bool isNewGame;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.isNewGame = isNewGame;

        return saveData;
    }

    public void LoadSaveData(SaveData data)
    {
        isNewGame = data.isNewGame;
    }
}