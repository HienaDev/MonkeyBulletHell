using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CraftingStation : MonoBehaviour
{
    [SerializeField] protected ItemType stationItemType;
    [SerializeField] protected GameObject player;
    protected List<CraftingRecipe> recipes;
    protected PlayerInventory playerInventory;

    protected virtual void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        recipes = Resources.LoadAll<CraftingRecipe>("").Where(recipe => recipe.ItemType == stationItemType).ToList();
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        UpdateCraftingUI(recipe);
    }

    protected abstract void UpdateCraftingUI(CraftingRecipe recipe);
}
