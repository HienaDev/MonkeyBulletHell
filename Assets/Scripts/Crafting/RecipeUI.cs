using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Transform materialContainer;
    [SerializeField] private Image[] materialIcons;
    [SerializeField] private TextMeshProUGUI[] materialQuantities;
    [SerializeField] private Button craftButton;
    [SerializeField] private Button equipButton1;
    [SerializeField] private Button equipButton2;
    [SerializeField] private Button equippedButton;

    private CraftingRecipe recipe;
    private PlayerInventory playerInventory;

    public void Setup(CraftingRecipe recipe, PlayerInventory playerInventory)
    {
        this.recipe = recipe;
        this.playerInventory = playerInventory;

        itemNameText.text = recipe.result.itemName;
        itemIcon.sprite = recipe.result.inventoryIcon;

        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (i < recipe.requiredMaterials.Count)
            {
                materialIcons[i].sprite = recipe.requiredMaterials[i].material.inventoryIcon;
                materialQuantities[i].text =  playerInventory.GetItemCount(recipe.requiredMaterials[i].material).ToString() + "/" + recipe.requiredMaterials[i].quantity.ToString();
                materialIcons[i].gameObject.SetActive(true);
                materialQuantities[i].gameObject.SetActive(true);
            }
            else
            {
                materialIcons[i].gameObject.SetActive(false);
                materialQuantities[i].gameObject.SetActive(false);
            }
        }

        craftButton.onClick.AddListener(() => TryCraft());
        equipButton1.onClick.AddListener(() => EquipWeapon(1));
        equipButton2.onClick.AddListener(() => EquipWeapon(2));

        UpdateUI();
    }

    private void TryCraft()
    {
        if (!recipe.isAlreadyCrafted && playerInventory.HasMaterials(recipe.requiredMaterials))
        {
            playerInventory.ConsumeMaterials(recipe.requiredMaterials);
            recipe.isAlreadyCrafted = true;
            UpdateUI();
            Debug.Log($"{recipe.result.itemName} crafted successfully.");
        }
        else if (recipe.isAlreadyCrafted)
        {
            Debug.Log($"{recipe.result.itemName} has already been crafted.");
        }
        else
        {
            Debug.Log("Insufficient materials to craft this item.");
        }
    }

    private void EquipWeapon(int slot)
    {
        if (recipe.isAlreadyCrafted)
        {
            if (slot == 2 && !playerInventory.IsWeaponEquippedInSlot(1))
            {
                Debug.LogWarning("Cannot equip in slot 2 if slot 1 is empty.");
                return;
            }

            playerInventory.EquipWeapon(slot, recipe.result);

            UpdateUI();
        }
    }


    public void UpdateUI()
    {
        if (recipe.isAlreadyCrafted)
            UpdateEquipStatus();
        else
            UpdateCraftStatus();
    }

    private void UpdateCraftStatus()
    {
        if (playerInventory.HasMaterials(recipe.requiredMaterials))
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;
        }

        craftButton.gameObject.SetActive(true);
        equipButton1.gameObject.SetActive(false);
        equipButton2.gameObject.SetActive(false);
        equippedButton.gameObject.SetActive(false);

        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (i < recipe.requiredMaterials.Count)
            {
                materialQuantities[i].text = playerInventory.GetItemCount(recipe.requiredMaterials[i].material).ToString() + "/" + recipe.requiredMaterials[i].quantity.ToString();
                materialQuantities[i].gameObject.SetActive(true);
            }
            else
            {
                materialQuantities[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateEquipStatus()
    {
        bool isEquippedInSlot1 = playerInventory.IsWeaponEquippedInSlot(1, recipe.result);
        bool isSlot1Occupied = playerInventory.IsSlotOccupied(1, ItemType.Weapon);
        bool isEquippedInSlot2 = playerInventory.IsWeaponEquippedInSlot(2, recipe.result);

        craftButton.gameObject.SetActive(false);

        if (isEquippedInSlot1 || isEquippedInSlot2)
        {
            equipButton1.gameObject.SetActive(false);
            equipButton2.gameObject.SetActive(false);
            equippedButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton1.gameObject.SetActive(true);
            equipButton2.gameObject.SetActive(true);
            equipButton2.interactable = isSlot1Occupied;

            equippedButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < materialIcons.Length; i++)
        {
            materialIcons[i].gameObject.SetActive(false);
            materialQuantities[i].gameObject.SetActive(false);
        }
    }
}