using UnityEngine;
using UnityEngine.UI;

public class RecipeUIWeapons : RecipeUI
{
    [SerializeField] private Button equipButton1;
    [SerializeField] private Button equipButton2;
    [SerializeField] private Button equippedButton;

    public override void Setup(CraftingRecipe recipe, PlayerInventory playerInventory, CraftingStation craftingStation)
    {
        base.Setup(recipe, playerInventory, craftingStation);

        equipButton1.onClick.AddListener(() => EquipWeapon(1));
        equipButton2.onClick.AddListener(() => EquipWeapon(2));

        UpdateUI();
    }

    protected override void UpdateCraftStatus()
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

    protected override void UpdateEquipStatus()
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
}