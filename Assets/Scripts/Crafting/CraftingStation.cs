using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class CraftingStation : MonoBehaviour
{
    [SerializeField] protected ItemType stationItemType;
    [SerializeField] protected GameObject craftingUI;
    [SerializeField] protected Transform itemGrid;
    [SerializeField] protected GameObject itemButtonPrefab;
    [SerializeField] protected Transform recipeDisplayParent;
    [SerializeField] protected GameObject recipePrefab;
    [SerializeField] protected GameObject player;

    protected List<CraftingRecipe> recipes;
    protected CraftingRecipe selectedRecipe;
    protected PlayerInventory playerInventory;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    protected virtual void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>(); 
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        recipes = Resources.LoadAll<CraftingRecipe>("").Where(recipe => recipe.ItemType == stationItemType).ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerIsNearStation())
        {
            craftingUI.SetActive(true);
            StopPlayerMovement();
            DisablePlayerControls();
            PopulateItemGrid();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            craftingUI.SetActive(false);
            ClearRecipePanel();
            EnablePlayerControls();
        }
    }

    private bool PlayerIsNearStation()
    {
        return Vector3.Distance(transform.position, playerInventory.transform.position) < 2f;
    }

    private void StopPlayerMovement()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("MovSpeed", 0f);
        }
    }

    private void DisablePlayerControls()
    {
        foreach (var script in player.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }

    private void EnablePlayerControls()
    {
        foreach (var script in player.GetComponents<MonoBehaviour>())
        {
            script.enabled = true;
        }
    }

    public virtual void PopulateItemGrid()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            bool canDisplay = recipe.isAlreadyCrafted || recipe.requiredMaterials.TrueForAll(req =>
                playerInventory.GetItemCount(req.material) >= 1);

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

    protected void OnItemButtonClicked(CraftingRecipe recipe)
    {
        SetRecipe(recipe);
    }

    protected void SetRecipe(CraftingRecipe recipe)
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
            recipeUIScript.Setup(selectedRecipe, playerInventory, this);
        }
        else
        {
            Debug.LogWarning("RecipeUI component not found on recipePrefab.");
        }
    }

    private void ClearRecipePanel()
    {
        foreach (Transform child in recipeDisplayParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshUI()
    {
        PopulateItemGrid();
        UpdateRecipeDisplay();
    }
}