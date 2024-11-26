using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Runtime.CompilerServices;
using NUnit.Framework;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private float health = 10;
    private float maxHealth;

    [SerializeField] private GameObject healthUI;
    [SerializeField] private Image healthImageUI;

    [SerializeField] private float entityHeight;
    [SerializeField] private GameObject destroyOnDeath;

    private Renderer rendererModel;
    private Material[] defaultMaterials;
    [SerializeField] private Material blinkMaterial;
    private Material[] blinkMaterials;

    [SerializeField] private UnityEvent doOnDeath;

    [SerializeField] private float gracePeriod = 0f;
    private float justGotDamaged = float.MinValue;
    [SerializeField] private float blinkDuration = 0.1f;
    private float justBlinked;
    private bool transparent = false;

    private bool immortal = false;

    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject gameOverScreen;
    private TAG_Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GetComponent<TAG_Player>();

        if (GetComponentInChildren<Renderer>() != null)
        {
            defaultMaterials = GetComponentInChildren<Renderer>().materials;
            rendererModel = GetComponentInChildren<Renderer>();

        }
        else
        {
            rendererModel = transform.parent.GetComponentInChildren<Renderer>();

            if(rendererModel != null)
            {
                defaultMaterials = rendererModel.materials;
   
            }
        }

        blinkMaterials = new Material[defaultMaterials.Length];

        for (int i = 0; i < blinkMaterials.Length; i++)
        {
            blinkMaterials[i] = blinkMaterial;
        }

        if (healthImageUI == null)
        {
            GameObject tempUI = Instantiate(healthUI, transform);
            healthImageUI = tempUI.GetComponentInChildren<TAG_HealthUI>().gameObject.GetComponent<Image>();
            tempUI.transform.position += new Vector3(0f, entityHeight, 0f);
        }


        maxHealth = health;

        healthImageUI.fillAmount = health / maxHealth;
    }

    private void FixedUpdate()
    {
        if (Time.time - justGotDamaged < gracePeriod)
        {

            if(Time.time - justBlinked > blinkDuration)
            {
                justBlinked = Time.time;
                if (!transparent)
                {
                    //foreach (Material mat in defaultMaterials)
                    //{
                    //    Debug.Log(mat.color + "before high");
                    //    mat.color = mat.color * 4f;
                    //    Debug.Log(mat.color + "after high");
                    //}

                    rendererModel.materials = defaultMaterials;
                }
                else
                {
                    //foreach (Material mat in defaultMaterials)
                    //{
                    //    Debug.Log(mat.color + "before low");
                    //    mat.color = mat.color * 0.25f;
                    //    Debug.Log(mat.color + "after low");
                    //}
                    rendererModel.materials = blinkMaterials;
                }

                transparent = !transparent;
            }
        }
        else if(!   transparent)
        {
            //Debug.Log("stop blink");
            //foreach (Material mat in defaultMaterials)
            //{
            //    mat.color = mat.color * 0.25f;

            //}
            rendererModel.materials = defaultMaterials;
            transparent = !transparent;
        }
    }

    public void DealDamage(float damage)
    {
        if(!immortal)
            if(Time.time - justGotDamaged > gracePeriod)
            {
                if(player != null)
                    if(playerInventory.GetEquippedArmor() != null)
                        damage *= playerInventory.GetEquippedArmor().damageReduction;

                health -= damage;
                
                Debug.Log("lost hp");

                if (health <= 0)
                {
                    if (doOnDeath != null)
                    {
                        doOnDeath.Invoke();
                    }

                    if(player != null)
                    {
                        GetComponentInParent<Animator>().SetTrigger("Die");
                        GetComponentInParent<PlayerMovement>().enabled = false;
                        GetComponentInParent<ShootingPlayer>().enabled = false;
                        GetComponentInParent<DashInDirection>().enabled = false;
                        GetComponent<Collider>().enabled = false;
                        gameOverScreen.SetActive(true);
                    }
                    else if (destroyOnDeath != null)
                    {
                        Destroy(destroyOnDeath);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    justGotDamaged = Time.time;
                }

                StartCoroutine(LoseHpUI());
            }


    }



    private IEnumerator LoseHpUI()
    {
        healthImageUI.fillAmount = health / maxHealth;


        if (GetComponentInChildren<Renderer>() != null)
            foreach (Material mat in defaultMaterials)
            {
                Debug.Log(mat);
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.white * 0.3f);
            }


        yield return new WaitForSeconds(0.05f);

        if (GetComponentInChildren<Renderer>() != null)
            foreach (Material mat in defaultMaterials)
            {
                mat.DisableKeyword("_EMISSION");
            }

    }

    public void Heal(float healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthImageUI.fillAmount = health / maxHealth;
    }

    public void SetImmortal(bool isImmortal)
    {

        immortal = isImmortal;

    }
}
