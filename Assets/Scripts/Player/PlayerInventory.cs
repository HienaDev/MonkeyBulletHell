using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxInventorySlots = 8;
    [SerializeField] private ToolSO axe; // ONLY FOR TESTING

    private ItemSO[] weaponSlots;
    private List<InventorySlot> inventoryItems;
    private ShootingPlayer shootingPlayerScript;
    private ItemSO equippedArmor;

    private void Start()
    {
        weaponSlots = new ItemSO[2];
        inventoryItems = new List<InventorySlot>();

        shootingPlayerScript = FindFirstObjectByType<ShootingPlayer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // ONLY FOR TESTING
        {
            AddItem(axe);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            DropSelectedItem();
        }
    }

    public void AddItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            if (weaponSlots[0] == null)
            {
                weaponSlots[0] = item;
                Debug.Log($"{item.itemName} added to weapon slot 1");
            }
            else if (weaponSlots[1] == null)
            {
                weaponSlots[1] = item;
                Debug.Log($"{item.itemName} added to weapon slot 2");
            }
            else
            {
                Debug.LogWarning("Both weapon slots are full.");
            }
        }
        else
        {
            InventorySlot existingSlot = inventoryItems.Find(slot => slot.Item == item);
            if (existingSlot != null && existingSlot.Quantity.HasValue)
            {
                existingSlot.IncreaseQuantity(1);
                Debug.Log($"{item.itemName} quantity increased to {existingSlot.Quantity}.");
            }
            else if (inventoryItems.Count < maxInventorySlots)
            {
                int? quantity = item.itemType == ItemType.Material ? 1 : (int?)null;
                inventoryItems.Add(new InventorySlot(item, quantity));
                Debug.Log($"{item.itemName} added to inventory.");
            }
            else
            {
                Debug.LogWarning("No more inventory slots available.");
            }
        }

        uiManager.UpdateInventoryDisplay();
    }

    public void RemoveItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            if (weaponSlots[0] == item)
            {
                weaponSlots[0] = null;
                Debug.Log($"{item.itemName} removed from weapon slot 1");
            }
            else if (weaponSlots[1] == item)
            {
                weaponSlots[1] = null;
                Debug.Log($"{item.itemName} removed from weapon slot 2");
            }
        }
        else
        {
            InventorySlot slot = inventoryItems.Find(s => s.Item == item);
            if (slot != null)
            {
                if (item.itemType == ItemType.Material && slot.Quantity > 1)
                {
                    slot.DecreaseQuantity(1);
                    Debug.Log($"{item.itemName} quantity decreased. Remaining: {slot.Quantity}");
                }
                else
                {
                    inventoryItems.Remove(slot);
                    Debug.Log($"{item.itemName} completely removed from inventory.");
                }
            }
            else
            {
                Debug.LogWarning("Item not found in inventory.");
            }
        }

        uiManager.UpdateInventoryDisplay();
    }

    public List<InventorySlot> RemoveAllMaterials()
    {
        List<InventorySlot> materialsToStore = new List<InventorySlot>();

        foreach (var slot in inventoryItems)
        {
            if (slot.Item.itemType == ItemType.Material)
            {
                materialsToStore.Add(slot);
            }
        }

        inventoryItems.RemoveAll(slot => slot.Item.itemType == ItemType.Material);

        uiManager.UpdateInventoryDisplay();

        return materialsToStore;
    }

    public bool MaterialAndToolSlotsFull()
    {
        return inventoryItems.Count >= maxInventorySlots;
    }

    public void DropSelectedItem()
    {
        int selectedSlotIndex = uiManager.GetSelectedSlotIndex();
        if (selectedSlotIndex == -1)
        {
            Debug.Log("No item selected or invalid slot.");
            return;
        }

        ItemSO selectedItem = GetItemAtSlot(selectedSlotIndex);
        if (selectedItem == null || selectedItem.itemType == ItemType.Weapon)
        {
            Debug.Log("Cannot drop weapon or no item found in selected slot.");
            return;
        }

        int lastSelectedIndex = selectedSlotIndex;

        // Drop the item's prefab slightly above the player's position with a specific rotation
        if (selectedItem.itemPrefab != null)
        {
            Vector3 dropPosition = transform.position + Vector3.up * 0.5f; // Adjust the height offset
            Quaternion dropRotation = Quaternion.Euler(0, 0, 0); // Set the desired rotation (e.g., no rotation)
            
            Instantiate(selectedItem.itemPrefab, dropPosition, dropRotation);
        }
        else
        {
            Debug.LogWarning("No prefab assigned for this item.");
        }

        // Remove the item or decrease its quantity if it's a material
        RemoveItem(selectedItem);

        // Update inventory display and find the next slot to select
        uiManager.UpdateInventoryDisplay();

        // Check if the current slot still has items
        ItemSO currentItem = GetItemAtSlot(lastSelectedIndex);
        if (currentItem != null && currentItem.itemType == ItemType.Material && GetItemCount(currentItem) > 0)
        {
            // Stay on the current slot if it’s a material with quantity left
            uiManager.SelectInventorySlot(lastSelectedIndex);
        }
        else
        {
            // Move to the closest available slot if the current slot is empty
            uiManager.SelectClosestAvailableSlot(lastSelectedIndex);
        }
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

            InventorySlot slot = inventoryItems.Find(s => s.Item == requirement.material);
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
                    inventoryItems.Remove(slot);
                    slot = inventoryItems.Find(s => s.Item == requirement.material);
                }
            }
        }

        uiManager.UpdateInventoryDisplay();
        NotifyRecipeUI();
    }

    public void EquipWeapon(int slot, ItemSO weapon)
    {
        if (weapon.itemType != ItemType.Weapon)
        {
            Debug.LogWarning("Cannot equip a non-weapon item in a weapon slot.");
            return;
        }

        if (slot == 1)
        {
            weaponSlots[0] = weapon;
            Debug.Log($"{weapon.itemName} equipped in slot 1");
        }
        else if (slot == 2)
        {
            weaponSlots[1] = weapon;
            Debug.Log($"{weapon.itemName} equipped in slot 2");
        }
        else
        {
            Debug.LogError($"Invalid slot index: {slot}. Cannot equip weapon.");
            return;
        }

        uiManager.UpdateInventoryDisplay();
        NotifyRecipeUI();
    }

    public void EquipArmor(ItemSO armor)
    {
        if (armor.itemType != ItemType.Armor)
        {
            Debug.LogWarning("Cannot equip a non-armor item as armor.");
            return;
        }

        equippedArmor = armor;
        Debug.Log($"{armor.itemName} equipped as armor.");
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

    public bool IsWeaponEquippedInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] != null : weaponSlots[1] != null;
    }

    public bool IsWeaponEquippedInSlot(int slot, ItemSO weapon)
    {
        return slot == 1 ? weaponSlots[0] == weapon : weaponSlots[1] == weapon;
    }

    public bool IsSlotOccupied(int slot, ItemType itemType)
    {
        if (slot == 1) return weaponSlots[0] != null && weaponSlots[0].itemType == itemType;
        if (slot == 2) return weaponSlots[1] != null && weaponSlots[1].itemType == itemType;
        return false;
    }

    public int GetItemCount(ItemSO item)
    {
        InventorySlot slot = inventoryItems.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    public ItemSO GetWeaponInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] : weaponSlots[1];
    }

    public void EquipWeapon(int slot)
    {
        Debug.Log("try equip weapon");
        if(weaponSlots[slot] is WeaponSO)
            shootingPlayerScript.SetWeapon((weaponSlots[slot] as WeaponSO));
    }

    public ItemSO GetItemAtSlot(int slot)
    {
        if (slot < 0) return null;

        if (slot < 2)
        {
            return weaponSlots[slot];
        }

        int adjustedSlot = slot - 2;
        if (adjustedSlot >= 0 && adjustedSlot < inventoryItems.Count)
        {
            return inventoryItems[adjustedSlot].Item;
        }

        return null;
    }

    public List<InventorySlot> GetInventoryItems()
    {
        return inventoryItems;
    }

    public ItemSO GetSelectedItem()
    {
        int selectedSlotIndex = uiManager.GetSelectedSlotIndex();
        return GetItemAtSlot(selectedSlotIndex);
    }

    public bool IsArmorEquipped(ItemSO armor)
    {
        return equippedArmor == armor;
    }

    public ItemSO GetEquippedArmor()
    {
        return equippedArmor;
    }
}