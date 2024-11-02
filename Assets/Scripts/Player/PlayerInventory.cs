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

            RemoveItem(selectedItem);

            if (items.Count == 0)
            {
                uiManager.ClearSelectedSlot();
            }
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