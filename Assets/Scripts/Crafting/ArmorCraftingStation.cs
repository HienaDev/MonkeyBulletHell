using UnityEngine;
using System.Collections.Generic;

public class ArmorCraftingStation : CraftingStation
{
    [SerializeField] private List<CraftingRecipe> armorRecipes;

    protected override void Start()
    {
        stationItemType = ItemType.Armor;
        base.Start();
        recipes = armorRecipes;
    }
}
