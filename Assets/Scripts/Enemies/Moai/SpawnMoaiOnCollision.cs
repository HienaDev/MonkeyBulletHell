using UnityEngine;

public class SpawnMoaiOnCollision : MonoBehaviour
{
    [SerializeField] private AttackPatterns moai;
    [SerializeField] private TriggerCameraShake rockDoor;

    [SerializeField] private LayerMask monkeyLayer; 




    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            moai.StartCombat();
            rockDoor.StartArenaShake();

            gameObject.SetActive(false);
        }

    }
}
