using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private float health = 10;
    private float maxHealth;
    [SerializeField] private Image healthImage;

    void Start()
    {
        maxHealth = health;

        healthImage.fillAmount = health / maxHealth;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        Debug.Log("lost hp");

        if (health <= 0) transform.parent.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        StartCoroutine(LoseHpUI());
    }

    private IEnumerator LoseHpUI()
    {

        float currentFill = healthImage.fillAmount;
        float finalFill = health / maxHealth;

        float lerpValue = 0;

        while (lerpValue <= 1)
        {
            Debug.Log(lerpValue);
            healthImage.fillAmount = Mathf.Lerp(currentFill, finalFill, lerpValue);

            lerpValue += Time.deltaTime;
            yield return null;
        }
    }
}
