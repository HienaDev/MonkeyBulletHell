using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<ItemSO> items;

    private int maxWeapons;
    private int maxMaterialsAndTools;

    private void Start()
    {
        items = new List<ItemSO>();
        maxWeapons = 2;
        maxMaterialsAndTools = 6;
    }

    public void AddItem(ItemSO item)
    {
        bool wasEmpty = items.Count == 0;
        items.Add(item);
        uiManager.UpdateInventoryDisplay();
        Debug.Log($"{item.itemName} added to inventory");

        if (wasEmpty)
        {
            uiManager.SelectFirstAvailableSlot();
        }
    }

    public void RemoveItem(ItemSO item)
    {
        int itemCount = GetItemCount(item);
        
        if (itemCount > 1)
        {
            // Apenas reduz a quantidade no inventário
            items.Remove(item); // Reduz uma instância
            uiManager.UpdateInventoryDisplay();
            Debug.Log($"{item.itemName} quantity decreased. Remaining: {itemCount - 1}");
        }
        else
        {
            // Remove o item completamente se restar apenas 1
            items.Remove(item);
            uiManager.UpdateInventoryDisplay();
            Debug.Log($"{item.itemName} removed from inventory");

            if (items.Count == 0)
            {
                uiManager.ClearSelectedSlot();
            }
            else
            {
                uiManager.SelectFirstAvailableSlot();
            }
        }
    }

    public ItemSO GetItem(string name)
    {
        return items.Find(item => item.itemName == name);
    }

    public bool WeaponSlotsFull()
    {
        return GetItemCountByType(ItemType.Weapon) >= maxWeapons;
    }

    public bool MaterialAndToolSlotsFull()
    {
        return GetItemCountByType(ItemType.Material) + GetItemCountByType(ItemType.Tool) >= maxMaterialsAndTools;
    }

    public void DropSelectedItem()
    {
        ItemSO selectedItem = uiManager.GetSelectedItem();

        if (selectedItem != null && (selectedItem.itemType == ItemType.Material || selectedItem.itemType == ItemType.Tool))
        {
            /*GameObject droppedItem = Instantiate(selectedItem.prefab, transform.position + transform.forward, Quaternion.identity);
            Debug.Log($"{selectedItem.itemName} dropped from inventory");*/

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DropSelectedItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Dictionary<ItemType, Dictionary<string, List<ItemSO>>> categorizedItems = new Dictionary<ItemType, Dictionary<string, List<ItemSO>>>();

            // Organize items by category and name
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

            // Print categorized items
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
        int count = 0;
        foreach (ItemSO i in items)
        {
            if (i == item)
                count++;
        }
        return count;
    }

    public int GetItemCountByType(ItemType itemType)
    {
        int count = 0;
        foreach (ItemSO i in items)
        {
            if (i.itemType == itemType)
                count++;
        }
        return count;
    }
}