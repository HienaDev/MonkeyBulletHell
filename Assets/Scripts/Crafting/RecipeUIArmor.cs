using UnityEngine;
using UnityEngine.UI;

public class RecipeUIArmor : RecipeUI
{
    [SerializeField] private Button equipButton;
    [SerializeField] private Button equippedButton;

    public override void Setup(CraftingRecipe recipe, Chest chest, PlayerInventory playerInventory, CraftingStation craftingStation)
    {
        base.Setup(recipe, chest, playerInventory, craftingStation);

        equipButton.onClick.AddListener(() => EquipArmor());

        UpdateUI();
    }

    protected override void UpdateCraftStatus()
    {
        if (chest.HasMaterials(recipe.requiredMaterials))
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;
        }

        craftButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        equippedButton.gameObject.SetActive(false);

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
        bool isEquipped = playerInventory.IsArmorEquipped(recipe.result);

        craftButton.gameObject.SetActive(false);

        if (isEquipped)
        {
            equipButton.gameObject.SetActive(false);
            equippedButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.gameObject.SetActive(true);
            equippedButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < materialIcons.Length; i++)
        {
            materialIcons[i].gameObject.SetActive(false);
            materialQuantities[i].gameObject.SetActive(false);
        }
    }

    protected override void TryCraft()
    {
        if (chest.HasMaterials(recipe.requiredMaterials))
        {
            chest.ConsumeMaterials(recipe.requiredMaterials);

            recipe.isAlreadyCrafted = true;

            craftingStation.RefreshUI();
            UpdateUI();

            Debug.Log($"{recipe.result.itemName} crafted successfully.");
        }
        else
        {
            Debug.Log("Insufficient materials to craft this armor.");
        }
    }

    private void EquipArmor()
    {
        if (recipe.isAlreadyCrafted)
        {
            playerInventory.EquipArmor(recipe.result);
            UpdateUI();
        }
    }
}