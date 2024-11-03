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
            // Cycle through inventory slots using the M key
            if (Input.GetKeyDown(KeyCode.M))
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
        occupiedSlots.Clear(); // Reset the list of occupied slots

        Dictionary<ItemSO, int> materialCounts = new Dictionary<ItemSO, int>();
        Dictionary<ItemSO, int> materialSlotIndex = new Dictionary<ItemSO, int>();

        // Populate material counts once for each material type
        foreach (var item in playerInventory.GetItems())
        {
            if (item.itemType == ItemType.Material)
            {
                if (!materialCounts.ContainsKey(item))
                {
                    materialCounts[item] = playerInventory.GetItemCount(item);
                }
            }
        }

        int weaponSlotIndex = 0;
        int otherSlotIndex = 2;

        foreach (var item in playerInventory.GetItems())
        {
            int slotIndex = -1; // Track the actual slot index used

            if (item.itemType == ItemType.Weapon && weaponSlotIndex < 2)
            {
                slotIndex = weaponSlotIndex;
                ShowInventorySlot(slotIndex);
                ShowInventoryIcon(slotIndex, item.inventoryIcon);
                weaponSlotIndex++;
            }
            else if ((item.itemType == ItemType.Material || item.itemType == ItemType.Tool) && otherSlotIndex < inventorySlots.Length)
            {
                if (item.itemType == ItemType.Material)
                {
                    // Check if this material has already been displayed in a slot
                    if (materialSlotIndex.ContainsKey(item))
                    {
                        // Update only the count in the existing slot
                        slotIndex = materialSlotIndex[item];
                        itemCounts[slotIndex].text = materialCounts[item].ToString();
                        itemCounts[slotIndex].gameObject.SetActive(true);
                    }
                    else
                    {
                        // Display the material in a new slot and store the slot index
                        slotIndex = otherSlotIndex;
                        ShowInventorySlot(slotIndex);
                        ShowInventoryIcon(slotIndex, item.inventoryIcon);
                        itemCounts[slotIndex].text = materialCounts[item].ToString();
                        itemCounts[slotIndex].gameObject.SetActive(true);

                        // Record the slot index for this material
                        materialSlotIndex[item] = slotIndex;
                        otherSlotIndex++;
                    }
                }
                else if (item.itemType == ItemType.Tool)
                {
                    // Display tools in slots 2-7 without duplication
                    slotIndex = otherSlotIndex;
                    ShowInventorySlot(slotIndex);
                    ShowInventoryIcon(slotIndex, item.inventoryIcon);
                    otherSlotIndex++;
                }
            }

            // Only add unique occupied slots to the list if a slot was assigned
            if (slotIndex != -1 && !occupiedSlots.Contains(slotIndex))
            {
                occupiedSlots.Add(slotIndex);
            }
        }

        // Ensure the selected slot stays within occupied slots
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