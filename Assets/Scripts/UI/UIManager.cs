using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

        foreach (var itemPair in playerInventory.GetInventoryItems())
        {
            if (otherSlotIndex >= inventorySlots.Length)
                break;

            ShowInventorySlot(otherSlotIndex);
            ShowInventoryIcon(otherSlotIndex, itemPair.Key.inventoryIcon);
            
            if (itemPair.Key.itemType == ItemType.Material)
            {
                itemCounts[otherSlotIndex].text = itemPair.Value.ToString();
                itemCounts[otherSlotIndex].gameObject.SetActive(true);
            }
            else
            {
                itemCounts[otherSlotIndex].gameObject.SetActive(false);
            }

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

    public int GetSelectedSlotIndex()
    {
        return selectedSlot >= 0 && selectedSlot < occupiedSlots.Count ? occupiedSlots[selectedSlot] : -1;
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

    public void SelectClosestAvailableSlot(int lastSelectedIndex)
    {
        if (occupiedSlots.Count == 0)
        {
            ClearSelectedSlot();
            return;
        }

        int closestIndex = occupiedSlots.FindIndex(slot => slot == lastSelectedIndex);
        
        if (closestIndex != -1)
        {
            selectedSlot = closestIndex;
        }
        else
        {
            int nearestSlotIndex = -1;
            int minDistance = int.MaxValue;

            for (int i = 0; i < occupiedSlots.Count; i++)
            {
                int distance = Mathf.Abs(occupiedSlots[i] - lastSelectedIndex);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSlotIndex = i;
                }
            }

            selectedSlot = nearestSlotIndex != -1 ? nearestSlotIndex : 0;
        }

        SelectInventorySlot(occupiedSlots[selectedSlot]);
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = -1;

        foreach (Image slot in inventorySlots)
        {
            slot.color = deselectedSlotColor;
        }
    }
}