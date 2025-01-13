using UnityEngine;

public class MoaiBoss : MonoBehaviour
{
    [SerializeField] private GameObject[] materialsToDrop;



    private void Start()
    {

    }

    public void DropMaterials()
    {
        foreach (GameObject material in materialsToDrop)
        {
            Vector2 randomPosition =  Random.insideUnitCircle * 5f;
            Instantiate(material, new Vector3(transform.position.x + randomPosition.x, transform.position.y + 2f, transform.position.z + randomPosition.y), Quaternion.identity);

        }
    }

    public void ResetBoss()
    {

    }
}
