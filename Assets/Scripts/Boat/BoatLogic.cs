using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoatLogic : MonoBehaviour
{

    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackDuration = 3f;

    private float justTeleported;

    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private bool needsWeapons = false;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform easterIslandLocation;


    [SerializeField] private GameObject noWeaponWarning;
    [SerializeField] private Color cantInteractColor;
    private Outline outline;

    [SerializeField] private UnityEvent doOnTeleport;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void TriggerTeleport()
    {

        if (needsWeapons)
            if (!inventory.PlayerHasWeaponEquipped())
            {
                StartCoroutine(CantUseBoat());
                return;
            }
                

        fadeScreen.TriggerFade(fadeDuration, blackDuration);
        doOnTeleport.Invoke();
        StartCoroutine(GoToEaster());
    }

    private IEnumerator CantUseBoat()
    {
        Color defaultColor = outline.OutlineColor;

        noWeaponWarning.SetActive(true);
        outline.OutlineColor = cantInteractColor;
        yield return new WaitForSeconds(2f);
        noWeaponWarning.SetActive(false);
        outline.OutlineColor = defaultColor;
    }

    private IEnumerator GoToEaster()
    {
        yield return new WaitForSeconds(2f);
        player.transform.position = easterIslandLocation.position;
    }
}
