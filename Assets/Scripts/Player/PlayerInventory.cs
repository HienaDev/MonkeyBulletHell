using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxInventorySlots = 8;
    [SerializeField] private ToolSO axe; // ONLY FOR TESTING

    private ItemSO[] weaponSlots;
    private List<InventorySlot> inventoryItems;

    private void Start()
    {
        weaponSlots = new ItemSO[2];
        inventoryItems = new List<InventorySlot>();
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
        NotifyAllRecipeUIs();
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
        NotifyAllRecipeUIs();
    }

    private void NotifyAllRecipeUIs()
    {
        RecipeUI[] recipeUIs = FindObjectsByType<RecipeUI>(FindObjectsSortMode.None);
        foreach (var recipeUI in recipeUIs)
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

    public ItemSO GetItemAtSlot(int slot)
    {
        if (slot < 2)
        {
            // Acessa o slot de armas (slot 0 é o weaponSlots[0], slot 1 é o weaponSlots[1])
            return weaponSlots[slot];
        }

        // Ajusta o índice para acessar o inventário de materiais e ferramentas
        int adjustedSlot = slot - 2;
        if (adjustedSlot < inventoryItems.Count)
        {
            return inventoryItems[adjustedSlot].Item;
        }

        return null; // Retorna null se o slot for inválido
    }

    public List<InventorySlot> GetInventoryItems()
    {
        return inventoryItems;
    }

    public ItemSO GetItem(string name)
    {
        foreach (var slot in inventoryItems)
        {
            if (slot.Item.itemName == name) return slot.Item;
        }

        foreach (var weapon in weaponSlots)
        {
            if (weapon != null && weapon.itemName == name) return weapon;
        }

        return null;
    }

    public Dictionary<ItemSO, int?> GetMaterialsAndTools()
    {
        Dictionary<ItemSO, int?> materialsAndToolsDict = new Dictionary<ItemSO, int?>();
        foreach (var slot in inventoryItems)
        {
            materialsAndToolsDict.Add(slot.Item, slot.Quantity);
        }
        return materialsAndToolsDict;
    }

    public bool HasItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            return weaponSlots[0] == item || weaponSlots[1] == item;
        }
        return inventoryItems.Exists(slot => slot.Item == item);
    }

    public int GetItemCountByType(ItemType itemType)
    {
        int count = 0;
        
        if (itemType == ItemType.Weapon)
        {
            count += weaponSlots[0] != null ? 1 : 0;
            count += weaponSlots[1] != null ? 1 : 0;
        }
        else
        {
            foreach (var slot in inventoryItems)
            {
                if (slot.Item.itemType == itemType)
                {
                    count += slot.Quantity ?? 1;
                }
            }
        }
        return count;
    }

    public bool WeaponSlotsFull()
    {
        return weaponSlots[0] != null && weaponSlots[1] != null;
    }
}