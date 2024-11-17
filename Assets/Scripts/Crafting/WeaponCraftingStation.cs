using UnityEngine;
using System.Collections.Generic;

public class WeaponCraftingStation : CraftingStation
{
    [SerializeField] private List<CraftingRecipe> weaponRecipes;

    protected override void Start()
    {
        stationItemType = ItemType.Weapon;
        base.Start();
        recipes = weaponRecipes;
    }
}