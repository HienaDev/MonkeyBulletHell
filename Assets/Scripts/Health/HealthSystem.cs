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
    private TAG_Player player;

    [SerializeField] private AttackPatterns attackPatterns;
    [SerializeField] private GameObject dropOnPhaseChange;

    [SerializeField] private Renderer renderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GetComponent<TAG_Player>();

        if (renderers == null)
        {
            if (GetComponentInChildren<Renderer>() != null)
            {
                defaultMaterials = GetComponentInChildren<Renderer>().materials;
                rendererModel = GetComponentInChildren<Renderer>();

            }
            else
            {
                rendererModel = transform.parent.GetComponentInChildren<Renderer>();

                if (rendererModel != null)
                {
                    defaultMaterials = rendererModel.materials;

                }
            }

            blinkMaterials = new Material[defaultMaterials.Length];

            for (int i = 0; i < blinkMaterials.Length; i++)
            {
                blinkMaterials[i] = blinkMaterial;
            }
        }
        else
        {

            defaultMaterials = renderers.materials;
            rendererModel = renderers;

            if (rendererModel != null)
            {
                defaultMaterials = rendererModel.materials;

            }


            blinkMaterials = new Material[defaultMaterials.Length];

            for (int i = 0; i < blinkMaterials.Length; i++)
            {
                blinkMaterials[i] = blinkMaterial;
            }
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

            if (Time.time - justBlinked > blinkDuration)
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
        else if (!transparent)
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
        if (!immortal)
            if (Time.time - justGotDamaged > gracePeriod)
            {
                if (player != null)
                    if (playerInventory.GetEquippedArmor() != null)
                        damage *= playerInventory.GetEquippedArmor().damageReduction;

                health -= damage;

                //Debug.Log("lost hp");

                if (health <= 0)
                {
                    health = 0;
                    if (doOnDeath != null)
                    {
                        doOnDeath.Invoke();
                    }

                    if (player != null)
                    {
                        PlayerDie();
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
                    if (attackPatterns != null)
                    {


                        if (health / maxHealth < 0.7f && health / maxHealth > 0.4f && attackPatterns.CurrentPhase != 2)
                        {
                            Debug.Log("Change to phase 2");
                            attackPatterns.ChangePhase(2);
                            DropItemOnPhase();
                        }
                        else if (health / maxHealth < 0.4f && health / maxHealth > 0.05f && attackPatterns.CurrentPhase != 3)
                        {
                            Debug.Log("Change to phase 3");
                            attackPatterns.ChangePhase(3);
                            DropItemOnPhase();
                        }
                        else if (health / maxHealth < 0.05f && attackPatterns.CurrentPhase != 4)
                        {
                            Debug.Log("Change to phase 4");
                            attackPatterns.ChangePhase(4);
                            DropItemOnPhase();
                        }
                    }
                }

                StartCoroutine(LoseHpUI());
            }


    }

    public void ResetBoss()
    {
        Heal(10000);
        SetImmortal(true);
    }

    public void DropItemOnPhase()
    {
        Vector2 randomPosition = Random.insideUnitCircle * 10f;
        Instantiate(dropOnPhaseChange, new Vector3(transform.position.x + randomPosition.x, transform.position.y, transform.position.z + randomPosition.y), Quaternion.identity);

    }

    public void PlayerDie()
    {
        GetComponentInParent<Animator>().SetTrigger("Die");
        GetComponentInParent<PlayerMovement>().enabled = false;
        GetComponentInParent<ShootingPlayer>().enabled = false;
        GetComponentInParent<DashInDirection>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public void RevivePlayer()
    {
        StartCoroutine(RevivePlayerCR());
    }

    private IEnumerator RevivePlayerCR()
    {

        yield return new WaitForSeconds(3f);
        Heal(maxHealth);
        GetComponentInParent<Animator>().SetTrigger("Revive");
        GetComponentInParent<PlayerMovement>().enabled = true;
        GetComponentInParent<ShootingPlayer>().enabled = true;
        GetComponentInParent<DashInDirection>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    public void ResetHealth()
    {
        health = maxHealth;
        healthImageUI.fillAmount = health / maxHealth;
    }

    private IEnumerator LoseHpUI()
    {
        healthImageUI.fillAmount = health / maxHealth;


        if (GetComponentInChildren<Renderer>() != null)
            foreach (Material mat in defaultMaterials)
            {
                //Debug.Log(mat);
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
