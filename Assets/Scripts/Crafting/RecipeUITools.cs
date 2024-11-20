using UnityEngine;

public class RecipeUITools : RecipeUI
{
    protected override void UpdateCraftStatus()
    {
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

        craftButton.interactable = chest.HasMaterials(recipe.requiredMaterials);
        craftButton.gameObject.SetActive(true);
    }

    protected override void UpdateEquipStatus()
    {
        craftButton.gameObject.SetActive(true);
    }

    protected override void TryCraft()
    {
        if (playerInventory.MaterialAndToolSlotsFull())
        {
            Debug.Log("Not enough room in the inventory to craft this item.");
            return;
        }

        if (chest.HasMaterials(recipe.requiredMaterials))
        {
            chest.ConsumeMaterials(recipe.requiredMaterials);
            playerInventory.AddItem(recipe.result);

            playerInventory.AddCraftedRecipe(recipe);

            craftingStation.RefreshUI();
            UpdateUI();

            Debug.Log($"{recipe.result.itemName} crafted successfully.");
        }
        else
        {
            Debug.Log("Insufficient materials to craft this tool.");
        }
    }
}