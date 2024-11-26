using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float spawnCooldown;
    private float justSpawned;
    [SerializeField] private float spawnRadius;

    [SerializeField] private Terrain terrain;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    private List<GameObject> enemies;

    private bool spawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = new List<GameObject>();
        justSpawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.time - justSpawned > spawnCooldown && spawning)
        {
            StartCoroutine(SpawnEnemyPrefab());
        }
        
    }
        
    private IEnumerator SpawnEnemyPrefab()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        while(!IsValidSpawnPosition(spawnPos)) 
        {
            spawnPos = GetRandomSpawnPosition();
            yield return null;
        }

        GameObject tempEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], transform);
        tempEnemy.transform.position = spawnPos;
        enemies.Add(tempEnemy);

        justSpawned = Time.time;
    }

    private Vector3 GetRandomSpawnPosition()
    {

        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        float minX = terrainPosition.x;
        float maxX = terrainPosition.x + terrainSize.x;

        float minZ = terrainPosition.z;
        float maxZ = terrainPosition.z + terrainSize.z;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        return new Vector3(randomX, terrainHeight, randomZ);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {

        Vector3 rayOrigin = new Vector3(position.x, position.y + 1f, position.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<TerrainCollider>() != null)
            {
                return true;
            }
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

        enemies.Clear();
    }
}
