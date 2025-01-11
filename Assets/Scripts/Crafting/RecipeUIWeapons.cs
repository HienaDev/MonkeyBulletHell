using UnityEngine;
using UnityEngine.UI;

public class RecipeUIWeapons : RecipeUI
{
    [SerializeField] private Button equipButton1;
    [SerializeField] private Button equipButton2;
    [SerializeField] private Button unequipButton;

    public override void Setup(CraftingRecipe recipe, Chest chest, PlayerInventory playerInventory, CraftingStation craftingStation)
    {
        base.Setup(recipe, chest, playerInventory, craftingStation);

        equipButton1.onClick.AddListener(() => EquipWeapon(1));
        equipButton2.onClick.AddListener(() => EquipWeapon(2));
        unequipButton.onClick.AddListener(() => UnequipWeapon());

        UpdateUI();
    }

    protected override void UpdateCraftStatus()
    {
        craftButton.interactable = chest.HasMaterials(recipe.requiredMaterials);
        craftButton.gameObject.SetActive(true);
        equipButton1.gameObject.SetActive(false);
        equipButton2.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);

        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (i < recipe.requiredMaterials.Count)
            {
                materialQuantities[i].text = chest.GetItemCount(recipe.requiredMaterials[i].material).ToString() + "/" + recipe.requiredMaterials[i].quantity.ToString();
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
        bool isEquippedInSlot2 = playerInventory.IsWeaponEquippedInSlot(2, recipe.result);
        bool isSlot1Occupied = playerInventory.IsWeaponEquippedInSlot(1);

        craftButton.gameObject.SetActive(false);

        if (isEquippedInSlot1 || isEquippedInSlot2)
        {
            equipButton1.gameObject.SetActive(false);
            equipButton2.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton1.gameObject.SetActive(true);
            equipButton2.gameObject.SetActive(true);

            // Desativa o botão do slot 2 se o slot 1 estiver vazio
            equipButton2.interactable = isSlot1Occupied;

            unequipButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < materialIcons.Length; i++)
        {
            materialIcons[i].gameObject.SetActive(false);
            materialQuantities[i].gameObject.SetActive(false);
        }
    }

    private void EquipWeapon(int slot)
    {
        if (playerInventory.IsRecipeCrafted(recipe))
        {
            
            if (slot == 2 && !playerInventory.IsWeaponEquippedInSlot(1))
            {
                Debug.LogWarning("Cannot equip in slot 2 if slot 1 is empty.");
                return;
            }

            playerInventory.EquipWeapon(slot, recipe.result);
            Tutorial.Instance.CraftedSlingshot(recipe.result as WeaponSO);
            Debug.Log($"{recipe.result.itemName} equipped in slot {slot}.");
            UpdateUI();
        }
    }

    private void UnequipWeapon()
    {
        int slotToUnequip = playerInventory.IsWeaponEquippedInSlot(1, recipe.result) ? 1 : 2;

        if (slotToUnequip != -1)
        {
            playerInventory.RemoveItem(recipe.result);
            Debug.Log($"{recipe.result.itemName} unequipped from slot {slotToUnequip}.");
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("This weapon is not equipped.");
        }
    }
}