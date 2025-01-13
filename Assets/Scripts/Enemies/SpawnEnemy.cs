using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float spawnCooldown;
    private float justSpawned;
    [SerializeField] private float spawnRadius;

    [SerializeField] private Transform positionToBeBias;

    [SerializeField] private Terrain terrain;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform player;

    private List<GameObject> enemies;

    private bool spawning = false;

    [SerializeField] private int enemiesToSpawnRightAway = 10;
    private bool spawnedInitial = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = new List<GameObject>();
        justSpawned = Time.time;
    }

    private void FixedUpdate()
    {
        if(spawning && !spawnedInitial)
        {
            spawnedInitial = true;
            for(int i = 0; i < enemiesToSpawnRightAway; i++)
            {
                StartCoroutine(SpawnEnemyPrefab());
            }
        }

        if (Time.time - justSpawned > spawnCooldown && spawning)
        {
            Debug.Log("Spawned");
            StartCoroutine(SpawnEnemyPrefab());
        }
    }
        
    private IEnumerator SpawnEnemyPrefab()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        while(!IsValidSpawnPosition(spawnPos)) 
        {
            Debug.Log("invalid position");
            spawnPos = GetRandomSpawnPosition();
            yield return null;
        }

        GameObject tempEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], transform);
        tempEnemy.transform.position = new Vector3(spawnPos.x, -1f, spawnPos.z);
        enemies.Add(tempEnemy);

        justSpawned = Time.time;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        float randomX = Mathf.Lerp(Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x), positionToBeBias.position.x, Random.value);
        float randomZ = Mathf.Lerp(Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z), positionToBeBias.position.z, Random.value);

        float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        return new Vector3(randomX, terrainHeight, randomZ);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        if(Vector3.Distance(position, transform.position) > spawnRadius) return false;

        Vector3 rayOrigin = new Vector3(position.x, position.y + 1f, position.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<TerrainCollider>() != null)
            {
                return true;
            }
            Debug.Log("detects collision");
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, spawnRadius);

        }
    }

    public void StartSpawning() => spawning = true;

    public void StopSpawning() => spawning = false;

    public void ClearEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        spawnedInitial = false;
        enemies.Clear();
    }
}
