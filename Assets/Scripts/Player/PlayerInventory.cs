using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<ItemSO> items;

    private int maxMaterialsAndTools;

    private void Start()
    {
        items = new List<ItemSO>(new ItemSO[8]);
        maxMaterialsAndTools = 6;
    }

    public void AddItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            // Adiciona a arma nos dois primeiros slots se um deles estiver vazio
            if (items[0] == null)
            {
                items[0] = item;
                Debug.Log($"{item.itemName} added to weapon slot 1");
            }
            else if (items[1] == null)
            {
                items[1] = item;
                Debug.Log($"{item.itemName} added to weapon slot 2");
            }
            else
            {
                Debug.LogWarning("Both weapon slots are full.");
            }
        }
        else
        {
            // Adiciona materiais/ferramentas nos slots restantes (do 3 ao 8)
            for (int i = 2; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    Debug.Log($"{item.itemName} added to material/tool slot {i - 1}");
                    break;
                }
            }
        }

        uiManager.UpdateInventoryDisplay();
    }

    public void RemoveItem(ItemSO item)
    {
        int index = items.IndexOf(item);

        if (index != -1)
        {
            items[index] = null;
            Debug.Log($"{item.itemName} removed from inventory slot {index + 1}");

            uiManager.UpdateInventoryDisplay();
        }
        else
        {
            Debug.LogWarning("Item not found in inventory.");
        }
    }

    public ItemSO GetItem(string name)
    {
        return items.Find(item => item.itemName == name);
    }

     public bool WeaponSlotsFull()
    {
        return items[0] != null && items[1] != null;
    }

    public bool MaterialAndToolSlotsFull()
    {
        int count = 0;

        for (int i = 2; i < items.Count; i++)
        {
            if (items[i] != null)
                count++;
        }

        return count >= maxMaterialsAndTools;
    }

    public void DropSelectedItem()
    {
        ItemSO selectedItem = uiManager.GetSelectedItem();

        if (selectedItem != null && (selectedItem.itemType == ItemType.Material || selectedItem.itemType == ItemType.Tool))
        {
            if (GetItemCount(selectedItem) > 1)
            {
                items.Remove(selectedItem);
                uiManager.UpdateItemQuantity(selectedItem);
                Debug.Log($"{selectedItem.itemName} quantity decreased. Remaining: {GetItemCount(selectedItem)}");
            }
            else
            {
                items.Remove(selectedItem);
                Debug.Log($"{selectedItem.itemName} removed from inventory");

                if (items.Count == 0)
                {
                    uiManager.ClearSelectedSlot();
                }
                else
                {
                    uiManager.SelectFirstAvailableSlot();
                }
            }

            uiManager.UpdateInventoryDisplay();
        }
        else
        {
            Debug.Log("Cannot drop this item, or no item selected.");
        }
    }

    public bool HasMaterials(List<ItemRequirement> requiredMaterials)
    {
        foreach (var requirement in requiredMaterials)
        {
            int playerMaterialCount = items.FindAll(item => item == requirement.material).Count;

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

            for (int i = items.Count - 1; i >= 0 && quantityToRemove > 0; i--)
            {
                if (items[i] == requirement.material)
                {
                    items.RemoveAt(i);
                    quantityToRemove--;
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
            if (items.Count == 0)
            {
                items.Add(weapon);
            }
            else if (items.Count >= 1)
            {
                items[0] = weapon;
            }
            else if (items.Count > 1)
            {
                items.Insert(0, weapon);
            }

            Debug.Log($"{weapon.itemName} equipped in slot 1");
        }
        else if (slot == 2)
        {
            if (items.Count < 2)
            {
                while (items.Count < 2)
                    items.Add(null);
                items[1] = weapon;
            }
            else if (items.Count >= 2)
            {
                items[1] = weapon;
            }

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
        if (slot == 1 && items.Count > 0)
            return items[0] != null;
        else if (slot == 2 && items.Count > 1)
            return items[1] != null;
        return false;
    }


    public void ReorganizeInventorySlots()
    {
        uiManager.UpdateInventoryDisplay();
    }

    public bool IsWeaponEquippedInSlot(int slot, ItemSO weapon)
    {
        if (slot == 1 && items.Count > 0)
        {
            return items[0] == weapon;
        }
        else if (slot == 2 && items.Count > 1)
        {
            return items[1] == weapon;
        }
        return false;
    }

    public bool IsSlotOccupied(int slot, ItemType itemType)
    {
        if (slot == 1 && items.Count > 0)
        {
            return items[0] != null && items[0].itemType == itemType;
        }
        else if (slot == 2 && items.Count > 1)
        {
            return items[1] != null && items[1].itemType == itemType;
        }
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
            Dictionary<ItemType, Dictionary<string, List<ItemSO>>> categorizedItems = new Dictionary<ItemType, Dictionary<string, List<ItemSO>>>();

            foreach (ItemSO item in items)
            {
                if (!categorizedItems.ContainsKey(item.itemType))
                {
                    categorizedItems[item.itemType] = new Dictionary<string, List<ItemSO>>();
                }

                if (!categorizedItems[item.itemType].ContainsKey(item.itemName))
                {
                    categorizedItems[item.itemType][item.itemName] = new List<ItemSO>();
                }

                categorizedItems[item.itemType][item.itemName].Add(item);
            }

            foreach (var category in categorizedItems)
            {
                Debug.Log($"Category: {category.Key}");

                foreach (var itemGroup in category.Value)
                {
                    Debug.Log($"- {itemGroup.Key} (Count: {itemGroup.Value.Count})");
                }
            }
        }
    }

    public List<ItemSO> GetItems()
    {
        return items;
    }

    public int GetItemCount(ItemSO item)
    {
        return items.FindAll(i => i == item).Count;
    }

    public int GetItemCountByType(ItemType itemType)
    {
        return items.FindAll(i => i != null && i.itemType == itemType).Count;
    }

    public bool HasItem(ItemSO item)
    {
        return items.Contains(item);
    }
}