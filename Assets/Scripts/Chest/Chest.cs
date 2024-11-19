using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Transform chestUI;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private MaterialSO[] materials;

    private List<InventorySlot> materialsInChest = new List<InventorySlot>();
    private PlayerInventory playerInventory;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
        chestUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerIsNearChest())
        {
            chestUI.gameObject.SetActive(true);
            StopPlayerMovement();
            DisablePlayerControls();
            PopulateChestUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chestUI.gameObject.SetActive(false);
            EnablePlayerControls();
        }

        // DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.E))
        {
            StoreAllMaterials();
        }

        // DEBUG ONLY - Fill chest with materials
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (var material in materials)
            {
                InventorySlot existingSlot = materialsInChest.Find(s => s.Item == material);

                if (existingSlot != null)
                {
                    existingSlot.IncreaseQuantity(99);
                }
                else
                {
                    materialsInChest.Add(new InventorySlot(material, 99));
                }
            }
        }
    }

    public void StoreAllMaterials()
    {
        List<InventorySlot> materialsToStore = playerInventory.RemoveAllMaterials();

        foreach (var slot in materialsToStore)
        {
            if (slot.Item is MaterialSO material)
            {
                InventorySlot existingSlot = materialsInChest.Find(s => s.Item == material);

                if (existingSlot != null)
                {
                    existingSlot.IncreaseQuantity(slot.Quantity ?? 0);
                }
                else
                {
                    materialsInChest.Add(new InventorySlot(material, slot.Quantity));
                }
            }
        }
    }

    private void PopulateChestUI()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in materialsInChest)
        {
            if (slot.Quantity <= 0)
            {
                Debug.LogWarning($"Material {slot.Item.itemName} has zero quantity. Skipping.");
                continue;
            }

            var itemButton = Instantiate(itemSlotPrefab, itemGrid);
            
            var icon = itemButton.GetComponentInChildren<Image>();
            var nameAndQuantityText = itemButton.GetComponentInChildren<TextMeshProUGUI>();

            if (icon == null || nameAndQuantityText == null)
            {
                Debug.LogError("Prefab structure is missing Image or TextMeshProUGUI component. Verify the prefab setup.");
                continue;
            }

            icon.sprite = slot.Item.inventoryIcon;
            nameAndQuantityText.text = $"{slot.Item.name}: {slot.Quantity}";
        }
    }

    private bool PlayerIsNearChest()
    {
        return Vector3.Distance(transform.position, player.transform.position) < 2f;
    }

    private void StopPlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("MovSpeed", 0f);
        }
    }

    private void DisablePlayerControls()
    {
        foreach (var script in player.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }

    private void EnablePlayerControls()
    {
        foreach (var script in player.GetComponents<MonoBehaviour>())
        {
            script.enabled = true;
        }
    }

    public int GetItemCount(ItemSO item)
    {
        InventorySlot slot = materialsInChest.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    public bool HasMaterials(List<ItemRequirement> requiredMaterials)
    {
        foreach (var requirement in requiredMaterials)
        {
            int playerMaterialCount = GetItemCount(requirement.material);

            if (playerMaterialCount < requirement.quantity)
            {
                Debug.Log($"Insufficient material: {requirement.material.itemName}. Required: {requirement.quantity}, Available: {playerMaterialCount}");
                return false;
            }
        }
        Debug.Log("All required materials are available.");
        return true;
    }

    public void ConsumeMaterials(List<ItemRequirement> requiredMaterials)
    {
        foreach (var requirement in requiredMaterials)
        {
            int quantityToRemove = requirement.quantity;

            InventorySlot slot = materialsInChest.Find(s => s.Item == requirement.material);
            while (quantityToRemove > 0 && slot != null)
            {
                if (slot.Quantity.HasValue && slot.Quantity > quantityToRemove)
                {
                    slot.DecreaseQuantity(quantityToRemove);
                    quantityToRemove = 0;
                }
                else
                {
                    quantityToRemove -= slot.Quantity ?? 0;
                    materialsInChest.Remove(slot);
                    slot = materialsInChest.Find(s => s.Item == requirement.material);
                }
            }
        }

        uiManager.UpdateInventoryDisplay();
        NotifyRecipeUI();
    }

    private void NotifyRecipeUI()
    {
        RecipeUI recipeUI = FindFirstObjectByType<RecipeUI>();
        if (recipeUI != null)
        {
            recipeUI.UpdateUI();
        }
    }
}