using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private ItemSO[] weaponSlots; // Array for two weapon slots
    private Dictionary<ItemSO, int> materialsAndTools; // Dictionary for materials/tools with counts
    private int maxMaterialsAndTools;

    private void Start()
    {
        weaponSlots = new ItemSO[2]; // Two dedicated slots for weapons
        materialsAndTools = new Dictionary<ItemSO, int>();
        maxMaterialsAndTools = 6;
    }

    public void AddItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            // Add weapon to first available weapon slot
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
            // Add materials/tools to dictionary
            if (materialsAndTools.ContainsKey(item))
            {
                materialsAndTools[item]++;
            }
            else
            {
                if (materialsAndTools.Count < maxMaterialsAndTools)
                {
                    materialsAndTools[item] = 1;
                    Debug.Log($"{item.itemName} added to inventory.");
                }
                else
                {
                    Debug.LogWarning("No more material/tool slots available.");
                }
            }
        }

        uiManager.UpdateInventoryDisplay();
    }

    public void RemoveItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            // Remove weapon from weapon slots
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
            // Remove material/tool from dictionary
            if (materialsAndTools.ContainsKey(item))
            {
                if (materialsAndTools[item] > 1)
                {
                    materialsAndTools[item]--;
                    Debug.Log($"{item.itemName} quantity decreased. Remaining: {materialsAndTools[item]}");
                }
                else
                {
                    materialsAndTools.Remove(item);
                    Debug.Log($"{item.itemName} completely removed from inventory.");
                }
            }
        }

        uiManager.UpdateInventoryDisplay();
    }

    public ItemSO GetItem(string name)
    {
        foreach (var item in materialsAndTools.Keys)
        {
            if (item.itemName == name) return item;
        }
        foreach (var item in weaponSlots)
        {
            if (item != null && item.itemName == name) return item;
        }
        return null;
    }

    public bool WeaponSlotsFull()
    {
        return weaponSlots[0] != null && weaponSlots[1] != null;
    }

    public bool MaterialAndToolSlotsFull()
    {
        return materialsAndTools.Count >= maxMaterialsAndTools;
    }

    public void DropSelectedItem()
    {
        ItemSO selectedItem = uiManager.GetSelectedItem();

        if (selectedItem != null)
        {
            if (selectedItem.itemType == ItemType.Weapon)
            {
                Debug.Log("Cannot drop weapons.");
                return; // Prevent dropping if the selected item is a weapon
            }
            
            // For materials/tools
            if (GetItemCount(selectedItem) > 1)
            {
                materialsAndTools[selectedItem]--;
                uiManager.UpdateItemQuantity(selectedItem);
                Debug.Log($"{selectedItem.itemName} quantity decreased. Remaining: {materialsAndTools[selectedItem]}");
            }
            else
            {
                materialsAndTools.Remove(selectedItem);
                Debug.Log($"{selectedItem.itemName} completely removed from inventory.");
            }

            uiManager.UpdateInventoryDisplay();
        }
        else
        {
            Debug.Log("No item selected or cannot drop this item.");
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

            while (quantityToRemove > 0 && materialsAndTools.ContainsKey(requirement.material))
            {
                if (materialsAndTools[requirement.material] > quantityToRemove)
                {
                    materialsAndTools[requirement.material] -= quantityToRemove;
                    quantityToRemove = 0;
                }
                else
                {
                    quantityToRemove -= materialsAndTools[requirement.material];
                    materialsAndTools.Remove(requirement.material);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DropSelectedItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Dictionary<ItemType, Dictionary<string, int>> categorizedItems = new Dictionary<ItemType, Dictionary<string, int>>();

            foreach (var item in materialsAndTools)
            {
                if (!categorizedItems.ContainsKey(item.Key.itemType))
                {
                    categorizedItems[item.Key.itemType] = new Dictionary<string, int>();
                }

                categorizedItems[item.Key.itemType][item.Key.itemName] = item.Value;
            }

            foreach (var category in categorizedItems)
            {
                Debug.Log($"Category: {category.Key}");

                foreach (var itemGroup in category.Value)
                {
                    Debug.Log($"- {itemGroup.Key} (Count: {itemGroup.Value})");
                }
            }
        }
    }

    public int GetItemCount(ItemSO item)
    {
        return item.itemType == ItemType.Weapon
            ? (weaponSlots[0] == item ? 1 : 0) + (weaponSlots[1] == item ? 1 : 0)
            : materialsAndTools.TryGetValue(item, out int count) ? count : 0;
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
            foreach (var item in materialsAndTools)
            {
                if (item.Key.itemType == itemType) count += item.Value;
            }
        }
        return count;
    }

    public bool HasItem(ItemSO item)
    {
        return item.itemType == ItemType.Weapon ? weaponSlots[0] == item || weaponSlots[1] == item : materialsAndTools.ContainsKey(item);
    }

    public ItemSO GetWeaponInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] : weaponSlots[1];
    }

    public Dictionary<ItemSO, int> GetMaterialsAndTools()
    {
        return materialsAndTools;
    }

    public ItemSO GetItemAtSlot(int slot)
    {
        if (slot < 2)
        {
            return GetWeaponInSlot(slot + 1);
        }
        else
        {
            int index = slot - 2;
            if (index >= 0 && index < materialsAndTools.Count)
            {
                var itemAtSlot = new List<ItemSO>(materialsAndTools.Keys)[index];
                return itemAtSlot;
            }
        }
        return null;
    }
}