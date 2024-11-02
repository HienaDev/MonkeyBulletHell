using UnityEngine;

public class MaterialOnGround : MonoBehaviour
{
    [SerializeField] private MaterialSO material;
    
    private PlayerInventory playerInventory;

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

    // If the player is inside the collider and presses the F key, gather the material
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            GatherMaterial();
        }
    }
}