using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BoatLogic : MonoBehaviour
{
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TextMeshProUGUI travelingUItext;
    [SerializeField] private string travellingText;
    [SerializeField] private GameObject travelingUI;
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

    [SerializeField] private GameObject mapUI;

    private void Start()
    {
        outline = GetComponent<Outline>();

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

        mapUI.SetActive(true);

        needsWeapons = false;


    }

    public void TeleportToIsland()
    {
        StartCoroutine(HandleTeleportWithFade());
    }

    private IEnumerator HandleTeleportWithFade()
    {
        fadeScreen.TriggerFade(fadeDuration, blackDuration);

        doOnTeleport.Invoke();

        yield return StartCoroutine(GoToEaster());

        yield return new WaitForSeconds(blackDuration - fadeDuration);
        travelingUI.GetComponent<Animator>().SetTrigger("Out");

        

        yield return new WaitForSeconds(2f);
        travelingUI.GetComponent<Animator>().SetTrigger("Reset");
        yield return null;
        travelingUI.SetActive(false);

        onTeleportComplete.Invoke();
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
        travelingUItext.text = travellingText;
        travelingUI.SetActive(true);
        yield return new WaitForSeconds(2f);

        player.transform.position = easterIslandLocation.position;
    }

    public bool CheckForWeapons()
    {
        if (needsWeapons)
        {
            if (!inventory.PlayerHasWeaponEquipped())
            {
                StartCoroutine(CantUseBoat());
                return false;
            }
        }

        return true;
    }
}