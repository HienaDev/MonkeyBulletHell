using UnityEngine;

public class MaterialOnGround : MonoBehaviour
{
    [SerializeField] private MaterialSO material;
    
    private PlayerInventory playerInventory;

    private bool playerInside = false;

    private void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void GatherMaterial()
    {
        Debug.Log($"Gathered {material.itemName}");
        
        if (playerInventory.MaterialAndToolSlotsFull())
        {
            Debug.Log("Inventory full");
            return;
        }
        
        playerInventory.AddItem(material);
        
        Destroy(gameObject);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            GatherMaterial();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

        }
    }
}