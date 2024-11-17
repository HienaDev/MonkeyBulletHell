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

        craftButton.interactable = playerInventory.HasMaterials(recipe.requiredMaterials);
        craftButton.gameObject.SetActive(true);
    }

    protected override void UpdateEquipStatus()
    {
        craftButton.gameObject.SetActive(true);
    }

    protected override void TryCraft()
    {
        if (playerInventory.HasMaterials(recipe.requiredMaterials))
        {
            playerInventory.ConsumeMaterials(recipe.requiredMaterials);
            playerInventory.AddItem(recipe.result);

            recipe.isAlreadyCrafted = true;
            
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