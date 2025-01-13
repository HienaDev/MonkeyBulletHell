using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] storyTiles;
    private GameObject currentTile;
    private int currentTileIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        background.SetActive(true);

        Time.timeScale = 0f;
        currentTileIndex = 0;
        currentTile = storyTiles[currentTileIndex];

        currentTile.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTile()
    {
        currentTileIndex++;
        currentTile.SetActive(false);

        if(currentTileIndex >= storyTiles.Length)
        {
            Time.timeScale = 1f;
            background.SetActive(false);
        }
        else
        {
            currentTile = storyTiles[currentTileIndex];
            currentTile.SetActive(true);
        }

    }
}
