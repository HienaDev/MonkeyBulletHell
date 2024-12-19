using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BoatLogic : MonoBehaviour
{
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TextMeshProUGUI travelingUI;
    [SerializeField] private string travellingText;
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
    [SerializeField] private UnityEvent onTeleportComplete;

    private void Start()
    {
        outline = GetComponent<Outline>();
        travelingUI.text = travellingText;
    }

    public void TriggerTeleport()
    {
        if (needsWeapons)
        {
            if (!inventory.PlayerHasWeaponEquipped())
            {
                StartCoroutine(CantUseBoat());
                return;
            }
        }

        needsWeapons = false;

        StartCoroutine(HandleTeleportWithFade());
    }

    private IEnumerator HandleTeleportWithFade()
    {
        fadeScreen.TriggerFade(fadeDuration, blackDuration);

        travelingUI.enabled = true;

        doOnTeleport.Invoke();

        yield return StartCoroutine(GoToEaster());

        yield return new WaitForSeconds(blackDuration - fadeDuration);

        travelingUI.enabled = false;

        onTeleportComplete?.Invoke();
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
        travelingUI.enabled = false;
        yield return new WaitForSeconds(2f);

        player.transform.position = easterIslandLocation.position;
    }
}