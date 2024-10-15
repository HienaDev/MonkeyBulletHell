using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField] private float damage = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TAG_Player>() != null)
        {
            Debug.Log("is enemy: " + other.name);
            other.GetComponent<HealthSystem>().DealDamage(damage);
        }
    }
}
