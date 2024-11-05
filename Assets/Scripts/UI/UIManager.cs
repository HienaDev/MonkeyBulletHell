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

        // Display weapons in the first two slots
        for (int i = 0; i < 2; i++)
        {
            var weapon = playerInventory.GetWeaponInSlot(i + 1);
            if (weapon != null)
            {
                ShowInventorySlot(i);
                ShowInventoryIcon(i, weapon.inventoryIcon);
                occupiedSlots.Add(i);
            }
        }

        int otherSlotIndex = 2;

        // Display materials and tools starting from slot index 2
        foreach (var item in playerInventory.GetMaterialsAndTools())
        {
            ShowInventorySlot(otherSlotIndex);
            ShowInventoryIcon(otherSlotIndex, item.Key.inventoryIcon);
            itemCounts[otherSlotIndex].text = item.Value.ToString();
            itemCounts[otherSlotIndex].gameObject.SetActive(true);

            occupiedSlots.Add(otherSlotIndex);
            otherSlotIndex++;
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
        if (selectedSlot >= 0 && selectedSlot < occupiedSlots.Count)
        {
            int slotIndex = occupiedSlots[selectedSlot];
            return playerInventory.GetItemAtSlot(slotIndex);
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