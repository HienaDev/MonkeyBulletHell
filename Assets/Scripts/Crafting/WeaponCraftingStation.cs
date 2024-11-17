using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponCraftingStation : CraftingStation
{
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private Transform recipeDisplayParent;
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
        if (Input.GetKeyDown(KeyCode.F) && PlayerIsNearStation())
        {
            craftingUI.SetActive(true);

            foreach (var script in player.GetComponents<MonoBehaviour>())
            {
                script.enabled = false;
            }

            PopulateItemGrid();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            craftingUI.SetActive(false);

            foreach (var script in player.GetComponents<MonoBehaviour>())
            {
                script.enabled = enabled;
            }
        }
    }

    private bool PlayerIsNearStation()
    {
        return Vector3.Distance(transform.position, playerInventory.transform.position) < 2f;
    }

    private void PopulateItemGrid()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            var itemButton = Instantiate(itemButtonPrefab, itemGrid);
            var buttonImage = itemButton.GetComponent<Image>();
            var button = itemButton.GetComponent<Button>();

            buttonImage.sprite = recipe.result.inventoryIcon;

            if (!recipe.isAlreadyCrafted)
            {
                buttonImage.color = new Color(0, 0, 0);
            }
            else
            {
                buttonImage.color = Color.white;
            }

            button.onClick.AddListener(() => OnItemButtonClicked(recipe));
        }
    }

    private void OnItemButtonClicked(CraftingRecipe recipe)
    {
        SetRecipe(recipe);
    }

    protected override void UpdateCraftingUI(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateRecipeDisplay();
    }

    private void UpdateRecipeDisplay()
    {
        foreach (Transform child in recipeDisplayParent)
        {
            Destroy(child.gameObject);
        }

        var recipeUI = Instantiate(recipePrefab, recipeDisplayParent);
        RecipeUI recipeUIScript = recipeUI.GetComponent<RecipeUI>();

        if (recipeUIScript != null)
        {
            recipeUIScript.Setup(selectedRecipe, playerInventory);
        }
        else
        {
            Debug.LogWarning("RecipeUI component not found on recipePrefab.");
        }
    }
}