using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Android;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private float health = 10;
    private float maxHealth;

    [SerializeField] private GameObject healthUI;
    [SerializeField] private Image healthImageUI;

    [SerializeField] private float entityHeight;
    [SerializeField] private GameObject destroyOnDeath;

    private Renderer rendererModel;
    private Material[] materials;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponentInChildren<Renderer>().materials != null)
            materials = GetComponentInChildren<Renderer>().materials;
        foreach (Material mat in materials)
        {
            Debug.Log(mat);

        }
        Debug.Log(materials);

        if (healthImageUI == null)
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
        healthImageUI.fillAmount = health / maxHealth;
        foreach(Material mat in materials)
        {
            Debug.Log(mat);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.white * 0.3f);
        }


        yield return new WaitForSeconds(0.05f);

        foreach (Material mat in materials)
        {
            mat.DisableKeyword("_EMISSION");
        }
        
    }

}
