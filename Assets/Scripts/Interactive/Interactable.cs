using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask monkeyLayer;
    private bool playerInside = false;

    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private float interactionCooldown = 0f;
    private float justInteracted;

    [SerializeField] private UnityEvent doOnInteract;
    private Outline outline;
    private PlayerInventory playerInventory;
    private MaterialSource materialSource;

    [SerializeField] private bool hitEffectToggle = false;
    [SerializeField] private GameObject hitEffect;

    private void Start()
    {
        justInteracted = float.MinValue;
        outline = GetComponent<Outline>();
        playerInventory = PlayerInventory.Instance;
        materialSource = GetComponent<MaterialSource>();
    }

    private void Update()
    {
        if (playerInside && outline != null)
        {
            if (CanPlayerInteract())
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }

        if (Input.GetKeyDown(interactKey) && Time.time - justInteracted > interactionCooldown && playerInside)
        {
            doOnInteract.Invoke();
            justInteracted = Time.time;

            if(hitEffectToggle && outline.enabled)
            {
                Instantiate(hitEffect, transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = true;

            if (outline != null)
            {
                outline.enabled = CanPlayerInteract();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = false;

            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    private bool CanPlayerInteract()
    {
        if (playerInventory == null) return false;

        ToolSO selectedTool = playerInventory.GetSelectedItem() as ToolSO;

        if (materialSource != null)
        {
            return materialSource.CanBreakWithoutTool() || materialSource.CanToolBreakMaterial(selectedTool);
        }

        return true;
    }
}