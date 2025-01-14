using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class MaterialOnGround : MonoBehaviour
{
    [SerializeField] private MaterialSO material;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    public void GatherMaterial()
    {
        if (playerInventory.ContainsMaterial(material))
        {
            playerInventory.AddItem(material);
            Debug.Log($"Added {material.itemName} to existing stack in inventory.");
        }
        else if (playerInventory.MaterialAndToolSlotsFull())
        {
            Debug.Log("Inventory full. Cannot pick up material.");
            return;
        }
        else
        {
            playerInventory.AddItem(material);
            Debug.Log($"Gathered {material.itemName}");
        }

        Destroy(gameObject);
    }

    public void SetMaterial(MaterialSO newMaterial)
    {
        material = newMaterial;
    }
}