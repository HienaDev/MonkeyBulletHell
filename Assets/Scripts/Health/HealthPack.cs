using UnityEngine;

public class HealthPack : MonoBehaviour
{

    [SerializeField] private float healAmount = 1f;

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
        if(other.GetComponent<TAG_Player>() != null)
        {
            other.GetComponent<HealthSystem>().Heal(healAmount);
            Destroy(gameObject, 0.1f);
        }
    }
}
