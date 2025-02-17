using UnityEngine;

public class ToolOnGround : MonoBehaviour
{
    [SerializeField] private ToolSO tool;
    
    private PlayerInventory playerInventory;

    private bool playerInside = false;

    private void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    public void GatherTool()
    {
        Debug.Log($"Gathered tool: {tool.itemName}");

        if (playerInventory.MaterialAndToolSlotsFull())
        {
            Debug.Log("Inventory full");
            return;
        }

        playerInventory.AddItem(tool);

        Destroy(gameObject);
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