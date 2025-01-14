using UnityEngine;
using System.Collections;

public class MaterialSource : MonoBehaviour
{
    [SerializeField] private MaterialSourceSO materialSource;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    private int hitsRemaining;
    private PlayerInventory playerInventory;
    private Vector3 originalPosition;

    private void Start()
    {
        hitsRemaining = materialSource.hitsToBreak;
        playerInventory = PlayerInventory.Instance;
        originalPosition = transform.localPosition;
    }

    public void GatherResource()
    {
        ToolSO selectedTool = playerInventory.GetSelectedItem() as ToolSO;

        if (CanBreakWithoutTool() || (selectedTool != null && CanToolBreakMaterial(selectedTool)))
        {
            if (hitsRemaining > 0)
            {
                StartCoroutine(Shake());

                hitsRemaining--;

                foreach (var material in materialSource.droppedMaterial)
                {
                    int materialAmount = materialSource.materialAmountPerHit * (selectedTool?.efficiency ?? 1);

                    for (int i = 0; i < materialAmount; i++)
                    {
                        if (!playerInventory.MaterialAndToolSlotsFull() || playerInventory.ContainsMaterial(material))
                        {
                            playerInventory.AddItem(material);
                        }
                        else
                        {
                            DropItemOnGround(material);
                        }
                    }

                    Debug.Log($"Gathered {material.itemName} x{materialAmount}");
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


    public bool CanToolBreakMaterial(ToolSO tool)
    {
        foreach (var source in materialSource.canBeBrokenWith)
        {
            if (source == tool)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanBreakWithoutTool()
    {
        return materialSource.canBeBrokenWith == null || materialSource.canBeBrokenWith.Length == 0;
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

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            randomOffset.y = 0;
            transform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}