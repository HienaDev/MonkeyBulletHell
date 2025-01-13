using UnityEngine;

public class HealthPack : MonoBehaviour
{

    [SerializeField] private float healAmount = 1f;



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TAG_Player>() != null)
        {
            if (other.GetComponent<HealthSystem>().IsFullHealth())
                return;

            other.GetComponent<HealthSystem>().Heal(healAmount);
            Destroy(gameObject, 0.1f);
        }
    }
}
