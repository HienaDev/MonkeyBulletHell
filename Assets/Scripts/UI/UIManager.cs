using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private Image[] inventorySlots;
    [SerializeField] private Image[] inventoryIcons;
    [SerializeField] private TextMeshProUGUI[] itemCounts;
    [SerializeField] private Color selectedSlotColor;
    [SerializeField] private Color deselectedSlotColor;

    [Header("Armor Level UI")]
    [SerializeField] private Image[] armorIcons;

    private int selectedSlot;
    private PlayerInventory playerInventory;
    private List<int> occupiedSlots = new List<int>();
    private bool wasEmpty;

    void Start()
    {
        selectedSlot = 0;
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        wasEmpty = true;
        HideInventoryIcons();
        HideInventoryNumbers();
        HideArmorIcons();
        SelectInventorySlot(-1);
    }

    void Update()
    {
        if (occupiedSlots.Count > 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                selectedSlot = (selectedSlot - 1 + occupiedSlots.Count) % occupiedSlots.Count;

                SelectInventorySlot(occupiedSlots[selectedSlot]);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                selectedSlot = (selectedSlot + 1) % occupiedSlots.Count;

                SelectInventorySlot(occupiedSlots[selectedSlot]);
            }
        }
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
        {
            inventorySlots[index].color = selectedSlotColor;

            ItemSO selectedItem = playerInventory.GetItemAtSlot(index);
            
            if (selectedItem is WeaponSO)
            {
                playerInventory.EquipWeapon(index);
            }
        }
    }

    public void UpdateUI()
    {
        UpdateArmorDisplay();
        UpdateInventory();
        
        if (occupiedSlots.Count > 0)
        {
            if (wasEmpty)
            {
                wasEmpty = false;
                SelectInventorySlot(occupiedSlots[0]);
                selectedSlot = 0;
            }
        }
        else
        {
            wasEmpty = true;
            ClearSelectedSlot();
        }
    }

    private void UpdateInventory()
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

        foreach (var slot in playerInventory.GetInventoryItems())
        {
            if (otherSlotIndex >= inventorySlots.Length)
                break;

            ShowInventorySlot(otherSlotIndex);
            ShowInventoryIcon(otherSlotIndex, slot.Item.inventoryIcon);

            if (slot.Item.ItemType == ItemType.Material && slot.Quantity > 0)
            {
                itemCounts[otherSlotIndex].text = slot.Quantity.ToString();
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
        else
        {
            ClearSelectedSlot();
        }
    }

    public void UpdateItemQuantity(ItemSO item)
    {
        if (occupiedSlots.Count > 0)
        {
            int slotIndex = occupiedSlots.FindIndex(slot => inventoryIcons[slot].sprite == item.inventoryIcon);
            if (slotIndex != -1)
            {
                var quantity = playerInventory.GetItemCount(item);
                itemCounts[slotIndex].text = quantity > 0 ? quantity.ToString() : string.Empty;
                itemCounts[slotIndex].gameObject.SetActive(quantity > 0);
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

        int nearestSlotIndex = -1;
        int minDistance = int.MaxValue;

        for (int i = 0; i < occupiedSlots.Count; i++)
        {
            int slot = occupiedSlots[i];
            int distance = Mathf.Abs(slot - lastSelectedIndex);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSlotIndex = i;
            }
        }

        if (nearestSlotIndex != -1)
        {
            selectedSlot = nearestSlotIndex;
            SelectInventorySlot(occupiedSlots[selectedSlot]);
        }
        else
        {
            ClearSelectedSlot();
        }
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = -1;

        foreach (Image slot in inventorySlots)
        {
            slot.color = deselectedSlotColor;
        }
    }

    public void UpdateArmorDisplay()
    {
        if (playerInventory.GetEquippedArmor() == null)
        {
            HideArmorIcons();
            return;
        }

        int activeIcons = Mathf.Clamp(Mathf.RoundToInt((1.0f - playerInventory.GetEquippedArmor().damageReduction) * 20), 0, armorIcons.Length);

        for (int i = 0; i < armorIcons.Length; i++)
        {
            armorIcons[i].enabled = i < activeIcons;
        }
    }

    public void HideArmorIcons()
    {
        foreach (Image icon in armorIcons)
            icon.enabled = false;
    }
}