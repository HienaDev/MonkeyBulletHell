using UnityEngine;

public class MaterialSource : MonoBehaviour
{
    [SerializeField] private MaterialSourceSO materialSource;
    private int hitsRemaining;
    private PlayerInventory playerInventory;

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

                foreach (var material in materialSource.droppedMaterial)
                {
                    if (playerInventory.MaterialAndToolSlotsFull())
                    {
                        DropItemOnGround(material);
                    }
                    else
                    {
                        playerInventory.AddItem(material);
                        Debug.Log($"Gathered {material.itemName}");
                    }
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

    private void DropItemOnGround(ItemSO item)
    {
        if (item.itemPrefab == null)
        {
            Debug.LogWarning($"No prefab assigned for the item: {item.itemName}");
            return;
        }

        Collider sourceCollider = GetComponent<Collider>();
        if (sourceCollider == null)
        {
            Debug.LogWarning("No collider found on MaterialSource. Defaulting to random position.");
            return;
        }

        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();

        float dropDistance = sourceCollider.bounds.extents.magnitude;
        Vector3 dropPosition = sourceCollider.bounds.center + randomDirection * dropDistance;

        if (Physics.Raycast(dropPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Floor")))
        {
            dropPosition.y = hit.point.y;
        }
        else
        {
            Debug.LogWarning("Floor not found! Defaulting to current Y position.");
            dropPosition.y = sourceCollider.bounds.center.y;
        }

        GameObject droppedItem = Instantiate(item.itemPrefab, dropPosition, Quaternion.identity);

        if (item is MaterialSO materialSO)
        {
            MaterialOnGround materialOnGround = droppedItem.GetComponent<MaterialOnGround>();
            if (materialOnGround != null)
            {
                materialOnGround.SetMaterial(materialSO);
            }
            else
            {
                Debug.LogWarning("Dropped item prefab is missing MaterialOnGround component.");
            }
        }
    }
}