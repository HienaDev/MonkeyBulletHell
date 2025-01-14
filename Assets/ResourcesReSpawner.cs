
using UnityEngine;
using System.Collections.Generic;

public class ResourcesReSpawner : MonoBehaviour
{
    [SerializeField] private GameObject resourcesEasterIslandParent;
    [SerializeField] private GameObject resourcesEasterIsland;
    [SerializeField] private GameObject materialsEasterIsland;
    private List<GameObject> resourcesSpawnsEasterIsland;

    [SerializeField] private GameObject materialsHubIsland;
    private List<GameObject> resourcesSpawnsHubIsland;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourcesSpawnsEasterIsland = new List<GameObject>();
        resourcesSpawnsHubIsland = new List<GameObject>();

        SpawnHubResources();
    }


    public void SpawnHubResources()
    {
        if(resourcesSpawnsHubIsland.Count > 0)
        {
            foreach(GameObject obj in resourcesSpawnsHubIsland)
            {
                Destroy(obj);
            }
            resourcesSpawnsHubIsland.Clear();
        }

        GameObject resource = Instantiate(materialsHubIsland, transform);
        resourcesSpawnsHubIsland.Add(resource);
    }

    public void SpawnEasterResources()
    {
        if (resourcesSpawnsEasterIsland.Count > 0)
        {
            foreach (GameObject obj in resourcesSpawnsEasterIsland)
            {
                Destroy(obj);
            }
            resourcesSpawnsEasterIsland.Clear();
        }

        GameObject resource = Instantiate(resourcesEasterIsland, resourcesEasterIslandParent.transform);
        resourcesSpawnsEasterIsland.Add(resource);

        resource = Instantiate(materialsEasterIsland, transform);
        resourcesSpawnsEasterIsland.Add(resource);
    }
}
