using System.Collections;
using UnityEngine;

public class BoatLogic : MonoBehaviour
{

    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackDuration = 3f;

    private float justTeleported;



    [SerializeField] private GameObject player;
    [SerializeField] private Transform easterIslandLocation;


    public void TriggerTeleport()
    {
        fadeScreen.TriggerFade(fadeDuration, blackDuration);
        StartCoroutine(GoToEaster());
    }


    private IEnumerator GoToEaster()
    {
        yield return new WaitForSeconds(2f);
        player.transform.position = easterIslandLocation.position;
    }
}
