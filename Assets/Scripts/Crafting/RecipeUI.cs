using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class RecipeUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI itemNameText;
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected Transform materialContainer;
    [SerializeField] protected Image[] materialIcons;
    [SerializeField] protected TextMeshProUGUI[] materialQuantities;
    [SerializeField] protected Button craftButton;

    protected CraftingRecipe recipe;
    protected Chest chest;
    protected PlayerInventory playerInventory;
    protected CraftingStation craftingStation;

    public virtual void Setup(CraftingRecipe recipe, Chest chest, PlayerInventory playerInventory, CraftingStation craftingStation)
    {
        this.recipe = recipe;
        this.chest = chest;
        this.craftingStation = craftingStation;
        this.playerInventory = playerInventory;

        itemNameText.text = recipe.result.itemName;
        itemIcon.sprite = recipe.result.inventoryIcon;

        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (i < recipe.requiredMaterials.Count)
            {
                materialIcons[i].sprite = recipe.requiredMaterials[i].material.inventoryIcon;
                materialQuantities[i].text = chest.GetItemCount(recipe.requiredMaterials[i].material).ToString() + "/" + recipe.requiredMaterials[i].quantity.ToString();
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
    }

    protected virtual void TryCraft()
    {
        if (!recipe.isAlreadyCrafted && chest.HasMaterials(recipe.requiredMaterials))
        {
            chest.ConsumeMaterials(recipe.requiredMaterials);
            recipe.isAlreadyCrafted = true;

            craftingStation.PopulateItemGrid();

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

    public void UpdateUI()
    {
        if (recipe.isAlreadyCrafted)
            UpdateEquipStatus();
        else
            UpdateCraftStatus();
    }

    protected abstract void UpdateEquipStatus();
    protected abstract void UpdateCraftStatus();
}