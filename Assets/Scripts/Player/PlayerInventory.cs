using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemSO> items;

    public void AddItem(ItemSO item)
    {
        items.Add(item);
        Debug.Log($"{item.itemName} added to inventory");
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        Debug.Log($"{item.itemName} removed from inventory");
    }

    public ItemSO GetItem(string name)
    {
        return items.Find(item => item.itemName == name);
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
}