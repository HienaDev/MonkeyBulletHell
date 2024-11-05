using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxInventorySlots = 8;
    [SerializeField] private ToolSO axe; // ONLY FOR TESTING

    private ItemSO[] weaponSlots;
    private List<KeyValuePair<ItemSO, int>> inventoryItems;

    private void Start()
    {
        weaponSlots = new ItemSO[2];
        inventoryItems = new List<KeyValuePair<ItemSO, int>>();
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Dictionary<ItemType, Dictionary<string, int>> categorizedItems = new Dictionary<ItemType, Dictionary<string, int>>();

            foreach (var itemPair in inventoryItems)
            {
                if (!categorizedItems.ContainsKey(itemPair.Key.itemType))
                {
                    categorizedItems[itemPair.Key.itemType] = new Dictionary<string, int>();
                }

                categorizedItems[itemPair.Key.itemType][itemPair.Key.itemName] = itemPair.Value;
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
        else if (item.itemType == ItemType.Material)
        {
            int index = inventoryItems.FindIndex(pair => pair.Key == item);
            if (index != -1)
            {
                inventoryItems[index] = new KeyValuePair<ItemSO, int>(item, inventoryItems[index].Value + 1);
                Debug.Log($"{item.itemName} quantity increased to {inventoryItems[index].Value}.");
            }
            else if (inventoryItems.Count < maxInventorySlots)
            {
                inventoryItems.Add(new KeyValuePair<ItemSO, int>(item, 1));
                Debug.Log($"{item.itemName} added to inventory as a new material.");
            }
            else
            {
                Debug.LogWarning("No more material slots available.");
            }
        }
        else if (item.itemType == ItemType.Tool)
        {
            if (inventoryItems.Count < maxInventorySlots)
            {
                inventoryItems.Add(new KeyValuePair<ItemSO, int>(item, 1));
                Debug.Log($"{item.itemName} added to inventory as a new tool instance.");
            }
            else
            {
                Debug.LogWarning("No more tool slots available.");
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
            int index = inventoryItems.FindIndex(pair => pair.Key == item);
            if (index != -1)
            {
                if (item.itemType == ItemType.Material && inventoryItems[index].Value > 1)
                {
                    inventoryItems[index] = new KeyValuePair<ItemSO, int>(item, inventoryItems[index].Value - 1);
                    Debug.Log($"{item.itemName} quantity decreased. Remaining: {inventoryItems[index].Value}");
                }
                else
                {
                    inventoryItems.RemoveAt(index);
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
        if (selectedItem == null)
        {
            Debug.Log("No item found in the selected slot.");
            return;
        }

        if (selectedItem.itemType == ItemType.Weapon)
        {
            Debug.Log("Cannot drop weapons.");
            return;
        }

        bool itemRemoved = false;

        if (selectedItem.itemType == ItemType.Material)
        {
            int index = inventoryItems.FindIndex(pair => pair.Key == selectedItem);
            if (index != -1 && inventoryItems[index].Value > 1)
            {
                inventoryItems[index] = new KeyValuePair<ItemSO, int>(selectedItem, inventoryItems[index].Value - 1);
                uiManager.UpdateItemQuantity(selectedItem);
                Debug.Log($"{selectedItem.itemName} quantity decreased. Remaining: {inventoryItems[index].Value}");
            }
            else
            {
                inventoryItems.RemoveAt(selectedSlotIndex - 2);
                itemRemoved = true;
                Debug.Log($"{selectedItem.itemName} completely removed from inventory.");
            }
        }
        else if (selectedItem.itemType == ItemType.Tool)
        {
            inventoryItems.RemoveAt(selectedSlotIndex - 2);
            itemRemoved = true;
            Debug.Log($"{selectedItem.itemName} instance removed from inventory.");
        }

        uiManager.UpdateInventoryDisplay();

        if (itemRemoved)
        {
            uiManager.SelectClosestAvailableSlot(selectedSlotIndex);
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

            int index = inventoryItems.FindIndex(pair => pair.Key == requirement.material);
            while (quantityToRemove > 0 && index != -1)
            {
                if (inventoryItems[index].Value > quantityToRemove)
                {
                    inventoryItems[index] = new KeyValuePair<ItemSO, int>(requirement.material, inventoryItems[index].Value - quantityToRemove);
                    quantityToRemove = 0;
                }
                else
                {
                    quantityToRemove -= inventoryItems[index].Value;
                    inventoryItems.RemoveAt(index);
                    index = inventoryItems.FindIndex(pair => pair.Key == requirement.material);
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
        int index = inventoryItems.FindIndex(pair => pair.Key == item);
        return index != -1 ? inventoryItems[index].Value : 0;
    }

    public ItemSO GetWeaponInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] : weaponSlots[1];
    }

    public ItemSO GetItemAtSlot(int slot)
    {
        if (slot < 2)
        {
            return GetWeaponInSlot(slot + 1);
        }

        int adjustedSlot = slot - 2;
        if (adjustedSlot < inventoryItems.Count)
        {
            return inventoryItems[adjustedSlot].Key;
        }

        return null;
    }

    public List<KeyValuePair<ItemSO, int>> GetInventoryItems()
    {
        return inventoryItems;
    }

    public ItemSO GetItem(string name)
    {
        foreach (var pair in inventoryItems)
        {
            if (pair.Key.itemName == name) return pair.Key;
        }
        foreach (var weapon in weaponSlots)
        {
            if (weapon != null && weapon.itemName == name) return weapon;
        }
        return null;
    }

    public Dictionary<ItemSO, int> GetMaterialsAndTools()
    {
        Dictionary<ItemSO, int> materialsAndToolsDict = new Dictionary<ItemSO, int>();
        foreach (var pair in inventoryItems)
        {
            materialsAndToolsDict.Add(pair.Key, pair.Value);
        }
        return materialsAndToolsDict;
    }

    public bool HasItem(ItemSO item)
    {
        return item.itemType == ItemType.Weapon
            ? weaponSlots[0] == item || weaponSlots[1] == item
            : inventoryItems.Exists(pair => pair.Key == item);
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
            foreach (var pair in inventoryItems)
            {
                if (pair.Key.itemType == itemType) count += pair.Value;
            }
        }
        return count;
    }

    public bool WeaponSlotsFull()
    {
        return weaponSlots[0] != null && weaponSlots[1] != null;
    }
}