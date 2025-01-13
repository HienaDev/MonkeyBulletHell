using UnityEngine;

public class MoaiBoss : MonoBehaviour
{
    [SerializeField] private GameObject[] materialsToDrop;

    public void DropMaterials()
    {
        foreach (GameObject material in materialsToDrop)
        {
            Vector2 randomPosition =  Random.insideUnitCircle * 5f;
            Instantiate(material, new Vector3(transform.position.x + randomPosition.x, transform.position.y, transform.position.z + randomPosition.y), Quaternion.identity);

        }
    }

    public void ResetBoss()
    {

    }
}
