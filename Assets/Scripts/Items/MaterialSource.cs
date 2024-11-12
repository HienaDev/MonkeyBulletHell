using UnityEngine;

public class MaterialSource : MonoBehaviour
{
    [SerializeField] private MaterialSourceSO materialSource;
    private int hitsRemaining;

    private PlayerInventory playerInventory;

    private bool playerInside = false;

    private void Start()
    {
        hitsRemaining = materialSource.hitsToBreak;
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void GatherResource()
    {
        ToolSO selectedTool = playerInventory.GetSelectedItem() as ToolSO;
        
        if (selectedTool != null && CanToolBreakMaterial(selectedTool))
        {
            if (hitsRemaining > 0)
            {
                hitsRemaining--;

                for (int i = 0; i < materialSource.droppedMaterial.Length; i++)
                {
                    Debug.Log($"Gathered {materialSource.droppedMaterial[i].itemName}");
                }

                for (int i = 0; i < materialSource.droppedMaterial.Length; i++)
                {
                    playerInventory.AddItem(materialSource.droppedMaterial[i]);
                }

                if (hitsRemaining <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.Log("You need a specific tool selected to break this material.");
        }
    }

    private bool CanToolBreakMaterial(ToolSO tool)
    {
        foreach (var source in tool.canBreakSources)
        {
            if (source == materialSource)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            GatherResource();
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