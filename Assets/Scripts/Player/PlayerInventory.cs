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
        items.Add(item);
        uiManager.UpdateInventoryDisplay();
        Debug.Log($"{item.itemName} added to inventory");
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        uiManager.UpdateInventoryDisplay();
        Debug.Log($"{item.itemName} removed from inventory");
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

    private void Update()
    {
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