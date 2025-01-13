using UnityEngine;

public class SpawnMoaiOnCollision : MonoBehaviour
{
    [SerializeField] private AttackPatterns moai;
    [SerializeField] private TriggerCameraShake rockDoor;
    [SerializeField] private LayerMask monkeyLayer;
    [SerializeField] private SpawnEnemy enemySpawner;

    private void OnTriggerEnter(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            moai.StartCombat();
            rockDoor.TriggerShake();
            gameObject.SetActive(false);

            enemySpawner.StopSpawning();
            enemySpawner.ClearEnemies();
        }

    }
}
