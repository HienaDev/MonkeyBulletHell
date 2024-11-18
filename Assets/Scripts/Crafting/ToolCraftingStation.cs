using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToolCraftingStation : CraftingStation
{
    [SerializeField] private List<CraftingRecipe> toolRecipes;

    protected override void Start()
    {
        stationItemType = ItemType.Tool;
        base.Start();
        recipes = toolRecipes;
    }

    public override void PopulateItemGrid()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            bool canDisplay = recipe.isAlreadyCrafted || recipe.requiredMaterials.TrueForAll(req =>
                chest.GetItemCount(req.material) >= 1);

            if (!canDisplay)
            {
                continue;
            }

            var itemButton = Instantiate(itemButtonPrefab, itemGrid);
            var buttonImage = itemButton.GetComponent<Image>();
            var button = itemButton.GetComponent<Button>();

            buttonImage.sprite = recipe.result.inventoryIcon;

            buttonImage.color = recipe.isAlreadyCrafted ? Color.white : new Color(0, 0, 0);

            button.onClick.AddListener(() => OnItemButtonClicked(recipe));
        }
    }
}