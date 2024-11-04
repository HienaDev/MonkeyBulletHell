using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WeaponCraftingStation : CraftingStation
{
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private GameObject recipePrefab;

    [SerializeField] private List<CraftingRecipe> weaponRecipes;

    private CraftingRecipe selectedRecipe;

    protected override void Start()
    {
        stationItemType = ItemType.Weapon;
        base.Start();

        recipes = weaponRecipes;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerIsNearStation())
        {
            craftingUI.SetActive(true);
            UpdateRecipeDisplay();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            craftingUI.SetActive(false);
        }
    }

    private bool PlayerIsNearStation()
    {
        return Vector3.Distance(transform.position, playerInventory.transform.position) < 2f;
    }

    protected override void UpdateCraftingUI(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
    }

    private void UpdateRecipeDisplay()
    {
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            var recipeUI = Instantiate(recipePrefab, recipeContainer);
            RecipeUI recipeUIScript = recipeUI.GetComponent<RecipeUI>();

            if (recipeUIScript != null)
            {
                recipeUIScript.Setup(recipe, playerInventory);
                recipeUIScript.UpdateUI();
            }
            else
            {
                Debug.LogWarning("RecipeUI component not found on recipePrefab.");
            }
        }
    }
}