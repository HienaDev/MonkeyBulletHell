using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private float health = 10;
    private float maxHealth;

    [SerializeField] private GameObject healthUI;
    [SerializeField] private Image healthImageUI;

    [SerializeField] private float entityHeight;
    [SerializeField] private GameObject destroyOnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(healthImageUI == null)
        {
            GameObject tempUI = Instantiate(healthUI, transform);
            healthImageUI = tempUI.GetComponentInChildren<TAG_HealthUI>().gameObject.GetComponent<Image>();
            tempUI.transform.position += new Vector3(0f, entityHeight, 0f);
        }
        

        maxHealth = health;

        healthImageUI.fillAmount = health/maxHealth;
    }


    public void DealDamage(float damage)
    {
        health -= damage;
        Debug.Log("lost hp");

        if (health <= 0)
        {
            if(destroyOnDeath != null)
            {
                Destroy(destroyOnDeath, 0.1f);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        StartCoroutine(LoseHpUI());
    }

    private IEnumerator LoseHpUI()
    {

        float currentFill = healthImageUI.fillAmount;
        float finalFill = health / maxHealth;

        float lerpValue = 0;

        while (lerpValue <= 1)
        {
            Debug.Log(lerpValue);
            healthImageUI.fillAmount = Mathf.Lerp(currentFill, finalFill, lerpValue);

            lerpValue += Time.deltaTime;
            yield return null;
        }

    }
}
