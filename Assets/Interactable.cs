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

    private void Start()
    {
        justInteracted = Time.time;
    }

    private void Update()
    {
        if(Input.GetKeyDown(interactKey) && Time.time - justInteracted > interactionCooldown && playerInside) 
        { 
            doOnInteract.Invoke();
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
}
