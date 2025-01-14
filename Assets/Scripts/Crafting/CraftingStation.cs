using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class CraftingStation : MonoBehaviour
{
    [SerializeField] protected ItemType stationItemType;
    [SerializeField] protected GameObject craftingUI;
    [SerializeField] protected Transform itemGrid;
    [SerializeField] protected GameObject itemButtonPrefab;
    [SerializeField] protected Transform recipeDisplayParent;
    [SerializeField] protected GameObject recipePrefab;
    [SerializeField] protected GameObject player;
    [SerializeField] private TextMeshProUGUI nothingToCraftMessage;

    private float fadeDuration = 0.1f;
    private CanvasGroup craftingUICanvasGroup;
    protected List<CraftingRecipe> recipes = new List<CraftingRecipe>();
    protected CraftingRecipe selectedRecipe;
    protected Chest chest;
    protected PlayerInventory playerInventory;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    protected virtual void Start()
    {
        chest = FindFirstObjectByType<Chest>();
        playerInventory = PlayerInventory.Instance;
        craftingUICanvasGroup = craftingUI.GetComponent<CanvasGroup>();

        if (craftingUICanvasGroup == null)
        {
            craftingUICanvasGroup = craftingUI.AddComponent<CanvasGroup>();
        }

        LoadRecipes();

        if (nothingToCraftMessage != null)
        {
            nothingToCraftMessage.gameObject.SetActive(false);
        }
    }

    private void LoadRecipes()
    {
        recipes.Clear();
        recipes = Resources.LoadAll<CraftingRecipe>("")
            .Where(recipe => recipe.ItemType == stationItemType)
            .ToList();
    }

    public void ToggleUI()
    {
        if (craftingUI.activeSelf)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    private void OpenUI()
    {
        if (craftingUI.activeSelf) return;

        Debug.Log("Opening crafting UI");
        craftingUI.SetActive(true);
        StopPlayerMovement();
        DisablePlayerControls();
        CheckAndPopulateGrid();
        StartCoroutine(FadeInUI());
    }

    private void CloseUI()
    {
        if (!craftingUI.activeSelf) return;

        Debug.Log("Closing crafting UI");
        StartCoroutine(FadeOutUI());
        ClearRecipePanel();
        EnablePlayerControls();
    }

    private IEnumerator FadeInUI()
    {
        float elapsedTime = 0;
        craftingUICanvasGroup.interactable = true;
        craftingUICanvasGroup.blocksRaycasts = true;

        while (elapsedTime < fadeDuration)
        {
            craftingUICanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        craftingUICanvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutUI()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            craftingUICanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        craftingUICanvasGroup.alpha = 0;
        craftingUI.SetActive(false);
        craftingUICanvasGroup.interactable = false;
        craftingUICanvasGroup.blocksRaycasts = false;
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

    private bool HasCraftableRecipes()
    {
        return recipes.Any(recipe =>
            recipe.requiredMaterials.All(req => chest.GetItemCount(req.material) >= 1) ||
            playerInventory.IsRecipeCrafted(recipe));
    }

    private void CheckAndPopulateGrid()
    {
        if (HasCraftableRecipes())
        {
            if (nothingToCraftMessage != null)
            {
                nothingToCraftMessage.gameObject.SetActive(false);
            }
            
            PopulateItemGrid();
        }
        else
        {
            if (nothingToCraftMessage != null)
            {
                nothingToCraftMessage.gameObject.SetActive(true);
            }

            ClearItemGrid();
        }
    }

    private void ClearItemGrid()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
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
            bool canDisplay = playerInventory.IsRecipeCrafted(recipe) ||
                            recipe.requiredMaterials.All(req => chest.GetItemCount(req.material) >= req.quantity);

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
            recipeUIScript.Setup(selectedRecipe, chest, playerInventory, this);
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
        CheckAndPopulateGrid();
        UpdateRecipeDisplay();
    }
}