using UnityEngine;

[RequireComponent(typeof(Interactable))]
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

}