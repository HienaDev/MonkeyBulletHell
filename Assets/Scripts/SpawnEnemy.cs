using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float spawnCooldown;
    private float justSpawned;
    [SerializeField] private float spawnRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        justSpawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Time.time - justSpawned > spawnCooldown)
        {
            SpawnEnemyPrefab();
        }
        
    }
        
    private void SpawnEnemyPrefab()
    {
        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius ;
        GameObject tempEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], transform);
        tempEnemy.transform.position = new Vector3(spawnPos.x, 0f, spawnPos.y);

        justSpawned = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
