using UnityEngine;

public class MaterialOnGround : MonoBehaviour
{
    [SerializeField]
    private MaterialSO material;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void GatherMaterial()
    {
        Debug.Log($"Gathered {material.itemName}");

        playerInventory.AddItem(material);
    
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            GatherMaterial();
        }
    }
}