using UnityEngine;

public class SpawnHealth : MonoBehaviour
{

    [SerializeField] private GameObject healthPack;


    public void SpawnHealthPack()
    {
        if(Random.Range(0f, 100f) < 33)
            Instantiate(healthPack, new Vector3(transform.position.x, 2f, transform.position.z), Quaternion.identity);
    }

}
