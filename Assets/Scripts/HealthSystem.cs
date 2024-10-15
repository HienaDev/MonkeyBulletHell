using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private float health = 10;
    private float maxHealth;

    [SerializeField] private GameObject healthUI;
    private Image healthImage;

    [SerializeField] private float entityHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject tempUI = Instantiate(healthUI, transform);
        healthImage = tempUI.GetComponentInChildren<TAG_HealthUI>().gameObject.GetComponent<Image>();
        tempUI.transform.position += new Vector3(0f, entityHeight, 0f);

        maxHealth = health;

        healthImage.fillAmount = health/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        Debug.Log("lost hp");
        healthImage.fillAmount = health / maxHealth;

        if (health <= 0) Destroy(transform.parent.gameObject, 0.1f);
    }
}
