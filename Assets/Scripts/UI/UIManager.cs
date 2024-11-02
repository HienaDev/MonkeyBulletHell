using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots;
    [SerializeField] private Image[] inventoryIcons;
    [SerializeField] private TextMeshProUGUI[] itemCounts;

    private int selectedSlot = 0;  // Start with first slot selected
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        HideInventoryIcons();
        HideInventoryNumbers();
        SelectInventorySlot(-1);
    }

    void Update()
    {
        // Navigate through inventory slots with arrow keys
        if (Input.GetKeyDown(KeyCode.RightArrow)) SelectInventorySlot((selectedSlot + 1) % 8);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectInventorySlot((selectedSlot + 7) % 8);

        // Refresh inventory UI display
        DisplayInventory();
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
        selectedSlot = index;
    }

    private void DisplayInventory()
    {
        HideInventoryIcons();
        HideInventoryNumbers();

        // Dictionary to track materials and their total counts
        Dictionary<ItemSO, int> materialCounts = new Dictionary<ItemSO, int>();

        // Populate material counts only once for each material type
        foreach (var item in playerInventory.items)
        {
            if (item.itemType == ItemType.Material)
            {
                if (!materialCounts.ContainsKey(item))
                    materialCounts[item] = playerInventory.GetItemCount(item);
            }
        }

        int weaponSlotIndex = 0;  // Track index for weapon slots (first two slots)
        int otherSlotIndex = 2;   // Track index for material and tool slots (slots 2-7)

        // Display items in the inventory UI
        foreach (var item in playerInventory.items)
        {
            if (item.itemType == ItemType.Weapon && weaponSlotIndex < 2)
            {
                // Display weapons in the first two slots
                ShowInventorySlot(weaponSlotIndex);
                ShowInventoryIcon(weaponSlotIndex, item.inventoryIcon);
                weaponSlotIndex++;
            }
            else if ((item.itemType == ItemType.Material || item.itemType == ItemType.Tool) && otherSlotIndex < inventorySlots.Length)
            {
                // Display materials/tools in slots 2-7
                if (item.itemType == ItemType.Material && materialCounts.ContainsKey(item))
                {
                    ShowInventorySlot(otherSlotIndex);
                    ShowInventoryIcon(otherSlotIndex, item.inventoryIcon);

                    // Show the total count for this material in one slot
                    itemCounts[otherSlotIndex].text = materialCounts[item].ToString();
                    itemCounts[otherSlotIndex].gameObject.SetActive(true);

                    // Remove from dictionary after showing, to avoid filling multiple slots
                    materialCounts.Remove(item);
                }
                else if (item.itemType == ItemType.Tool)
                {
                    ShowInventorySlot(otherSlotIndex);
                    ShowInventoryIcon(otherSlotIndex, item.inventoryIcon);
                }

                otherSlotIndex++;
            }
        }
    }
}