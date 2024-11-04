using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots;
    [SerializeField] private Image[] inventoryIcons;
    [SerializeField] private TextMeshProUGUI[] itemCounts;
    [SerializeField] private Color selectedSlotColor;
    [SerializeField] private Color deselectedSlotColor;

    private int selectedSlot;
    private PlayerInventory playerInventory;
    private List<int> occupiedSlots = new List<int>();

    void Start()
    {
        selectedSlot = 0;
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        HideInventoryIcons();
        HideInventoryNumbers();
        SelectInventorySlot(-1);
    }

    void Update()
    {
        if (occupiedSlots.Count > 0)
        {
            // Cycle through inventory slots using the M key
            if (Input.GetKeyDown(KeyCode.M))
            {
                selectedSlot = (selectedSlot + 1) % occupiedSlots.Count;
                SelectInventorySlot(occupiedSlots[selectedSlot]);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            UpdateInventoryDisplay();
        }

        Debug.Log($"Selected item: {GetSelectedItem()}");
    }

    public void HideInventoryIcons()
    {
        foreach (Image icon in inventoryIcons)
            icon.enabled = false;
    }

    public void HideInventoryNumbers()
    {
        foreach (TextMeshProUGUI count in itemCounts)
            count.gameObject.SetActive(false);
    }

    public void ShowInventoryIcon(int index, Sprite icon)
    {
        inventoryIcons[index].sprite = icon;
        inventoryIcons[index].enabled = true;
    }

    public void ShowInventorySlot(int index)
    {
        inventorySlots[index].enabled = true;
    }

    public void SelectInventorySlot(int index)
    {
        foreach (Image slot in inventorySlots)
            slot.color = deselectedSlotColor;

        if (index != -1)
            inventorySlots[index].color = selectedSlotColor;
    }

    public void UpdateInventoryDisplay()
    {
        DisplayInventory();
    }

    private void DisplayInventory()
    {
        HideInventoryIcons();
        HideInventoryNumbers();
        occupiedSlots.Clear();

        Dictionary<ItemSO, int> materialCounts = new Dictionary<ItemSO, int>();
        Dictionary<ItemSO, int> materialSlotIndex = new Dictionary<ItemSO, int>();

        foreach (var item in playerInventory.GetItems())
        {
            if (item != null && item.itemType == ItemType.Material)
            {
                if (!materialCounts.ContainsKey(item))
                {
                    materialCounts[item] = playerInventory.GetItemCount(item);
                }
            }
        }

        if (playerInventory.GetItems().Count > 0)
        {
            if (playerInventory.GetItems().Count >= 1 && playerInventory.GetItems()[0] != null && playerInventory.GetItems()[0].itemType == ItemType.Weapon)
            {
                ShowInventorySlot(0);
                ShowInventoryIcon(0, playerInventory.GetItems()[0].inventoryIcon);
                occupiedSlots.Add(0);
            }

            if (playerInventory.GetItems().Count >= 2 && playerInventory.GetItems()[1] != null && playerInventory.GetItems()[1].itemType == ItemType.Weapon)
            {
                ShowInventorySlot(1);
                ShowInventoryIcon(1, playerInventory.GetItems()[1].inventoryIcon);
                occupiedSlots.Add(1);
            }
        }

        int otherSlotIndex = 2;
        foreach (var item in playerInventory.GetItems())
        {
            if (item == null || item.itemType == ItemType.Weapon) continue;

            int slotIndex = -1;

            if (item.itemType == ItemType.Material)
            {
                if (materialSlotIndex.ContainsKey(item))
                {
                    slotIndex = materialSlotIndex[item];
                    itemCounts[slotIndex].text = materialCounts[item].ToString();
                    itemCounts[slotIndex].gameObject.SetActive(true);
                }
                else
                {
                    slotIndex = otherSlotIndex;
                    ShowInventorySlot(slotIndex);
                    ShowInventoryIcon(slotIndex, item.inventoryIcon);
                    itemCounts[slotIndex].text = materialCounts[item].ToString();
                    itemCounts[slotIndex].gameObject.SetActive(true);

                    materialSlotIndex[item] = slotIndex;
                    otherSlotIndex++;
                }
            }
            else if (item.itemType == ItemType.Tool)
            {
                slotIndex = otherSlotIndex;
                ShowInventorySlot(slotIndex);
                ShowInventoryIcon(slotIndex, item.inventoryIcon);
                otherSlotIndex++;
            }

            if (slotIndex != -1 && !occupiedSlots.Contains(slotIndex))
            {
                occupiedSlots.Add(slotIndex);
            }
        }

        if (occupiedSlots.Count > 0)
        {
            SelectInventorySlot(occupiedSlots[selectedSlot % occupiedSlots.Count]);
        }
    }

    public void UpdateItemQuantity(ItemSO item)
    {
        if (occupiedSlots.Count > 0)
        {
            int slotIndex = occupiedSlots.FindIndex(slot => inventoryIcons[slot].sprite == item.inventoryIcon);
            if (slotIndex != -1)
            {
                itemCounts[slotIndex].text = playerInventory.GetItemCount(item).ToString();
            }
        }
    }

    public ItemSO GetSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < playerInventory.GetItems().Count)
        {
            return playerInventory.GetItems()[selectedSlot];
        }

        return null;
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = -1;

        foreach (Image slot in inventorySlots)
        {
            slot.color = deselectedSlotColor;
        }
    }

    public void SelectFirstAvailableSlot()
    {
        if (occupiedSlots.Count > 0)
        {
            selectedSlot = 0;
            SelectInventorySlot(occupiedSlots[selectedSlot]);
        }
        else
        {
            ClearSelectedSlot();
        }
    }
}