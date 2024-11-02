using UnityEngine;

public class MaterialSource : MonoBehaviour
{
    [SerializeField]
    private MaterialSourceSO materialSource;
    private int hitsRemaining;

    private PlayerInventory playerInventory;

    private void Start()
    {
        hitsRemaining = materialSource.hitsToBreak;
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void GatherResource()
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            GatherResource();
        }
    }
}