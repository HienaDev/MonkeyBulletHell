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
    protected PlayerInventory playerInventory;
    protected WeaponCraftingStation craftingStation;

    public virtual void Setup(CraftingRecipe recipe, PlayerInventory playerInventory, WeaponCraftingStation craftingStation)
    {
        this.recipe = recipe;
        this.playerInventory = playerInventory;
        this.craftingStation = craftingStation;

        itemNameText.text = recipe.result.itemName;
        itemIcon.sprite = recipe.result.inventoryIcon;

        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (i < recipe.requiredMaterials.Count)
            {
                materialIcons[i].sprite = recipe.requiredMaterials[i].material.inventoryIcon;
                materialQuantities[i].text = playerInventory.GetItemCount(recipe.requiredMaterials[i].material).ToString() + "/" + recipe.requiredMaterials[i].quantity.ToString();
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
        if (!recipe.isAlreadyCrafted && playerInventory.HasMaterials(recipe.requiredMaterials))
        {
            // Consumir materiais e marcar como craftado
            playerInventory.ConsumeMaterials(recipe.requiredMaterials);
            recipe.isAlreadyCrafted = true;

            // Atualizar a UI da grelha após crafting
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