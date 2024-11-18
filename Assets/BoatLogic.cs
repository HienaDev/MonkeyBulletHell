using System.Collections;
using UnityEngine;

public class BoatLogic : MonoBehaviour
{
    [SerializeField] private LayerMask monkeyLayer;
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackDuration = 3f;

    private bool playerInside = false;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform easterIslandLocation;

    private void Update()
    {

        if(playerInside && Input.GetKeyDown(KeyCode.F))
        {
            fadeScreen.TriggerFade(fadeDuration, blackDuration);
            StartCoroutine(GoToEaster());
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {

        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = false;
        }
    }

    private IEnumerator GoToEaster()
    {
        yield return new WaitForSeconds(2f);
        player.transform.position = easterIslandLocation.position;
    }
}
