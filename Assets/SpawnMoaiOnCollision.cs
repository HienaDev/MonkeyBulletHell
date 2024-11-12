using UnityEngine;

public class SpawnMoaiOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject moai;

    private void OnCollisionEnter(Collision collision)
    {
        moai.SetActive(true);
    }
}
