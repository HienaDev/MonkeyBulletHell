using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArmorCraftingStation : CraftingStation
{
    [SerializeField] private List<CraftingRecipe> armorRecipes;

    protected override void Start()
    {
        stationItemType = ItemType.Armor;
        base.Start();
        recipes = armorRecipes;
    }

    public override void PopulateItemGrid()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            bool canDisplay = playerInventory.IsRecipeCrafted(recipe) || recipe.requiredMaterials.TrueForAll(req =>
                chest.GetItemCount(req.material) >= 1);

            if (!canDisplay)
            {
                continue;
            }

            var itemButton = Instantiate(itemButtonPrefab, itemGrid);
            var buttonImage = itemButton.GetComponent<Image>();
            var button = itemButton.GetComponent<Button>();

            buttonImage.sprite = recipe.result.inventoryIcon;

            buttonImage.color = playerInventory.IsRecipeCrafted(recipe) ? Color.white : new Color(0, 0, 0);

            button.onClick.AddListener(() => OnItemButtonClicked(recipe));
        }
    }
}
