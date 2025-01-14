using UnityEngine;
using System.Collections.Generic;

public class ToolCraftingStation : CraftingStation
{
    [SerializeField] private List<CraftingRecipe> toolRecipes;

    protected override void Start()
    {
        stationItemType = ItemType.Tool;
        base.Start();
        recipes = toolRecipes;
    }
}