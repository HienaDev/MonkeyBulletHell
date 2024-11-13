using UnityEngine;

public class SpawnMoaiOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject moai;
    [SerializeField] private GameObject rockDoor;

    private void OnCollisionEnter(Collision collision)
    {
        moai.SetActive(true);
        rockDoor.SetActive(true);
    }
}
