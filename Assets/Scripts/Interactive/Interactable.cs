using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask monkeyLayer;
    private bool playerInside = false;

    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private float interactionCooldown = 0f;
    private float justInteracted;

    [SerializeField] private UnityEvent doOnInteract;

    private Func<bool> interactionCondition;
    private Outline outline;
    private bool canInteract = true;

    private void Start()
    {
        justInteracted = Time.time;
        outline = GetComponent<Outline>();
        UpdateInteractionState();
    }

    private void Update()
    {
        UpdateInteractionState();

        if (Input.GetKeyDown(interactKey) && Time.time - justInteracted > interactionCooldown && playerInside && canInteract)
        {
            justInteracted = Time.time;
            doOnInteract.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = true;
            if (outline != null)
                outline.enabled = canInteract;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((monkeyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerInside = false;
            if (outline != null)
                outline.enabled = false;
        }
    }

    private void UpdateInteractionState()
    {
        bool newInteractionState = interactionCondition == null || interactionCondition.Invoke();

        if (canInteract != newInteractionState)
        {
            canInteract = newInteractionState;

            if (outline != null)
            {
                outline.enabled = canInteract && playerInside;
            }
        }
    }

    public UnityEvent GetDoOnInteract()
    {
        return doOnInteract;
    }

    public void SetInteractionCondition(Func<bool> condition)
    {
        interactionCondition = condition;
    }
}