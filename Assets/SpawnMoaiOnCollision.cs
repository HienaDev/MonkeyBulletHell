using UnityEngine;

public class SpawnMoaiOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject moai;
    [SerializeField] private GameObject rockDoor;

    [SerializeField] private LayerMask monkeyLayer; 




    private void OnTriggerEnter(Collider other)
    {

        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            moai.SetActive(true);
            rockDoor.SetActive(true);

            gameObject.SetActive(false);
        }

    }
}
