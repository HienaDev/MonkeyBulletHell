using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject chestUI;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TextMeshProUGUI nothingHereMessage;
    [SerializeField] private Animator animator;

    private float fadeDuration = 0.1f;
    private CanvasGroup chestUICanvasGroup;

    private List<InventorySlot> materialsInChest = new List<InventorySlot>();
    private PlayerInventory playerInventory;

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();

        chestUICanvasGroup = chestUI.GetComponent<CanvasGroup>();
        if (chestUICanvasGroup == null)
        {
            chestUICanvasGroup = chestUI.AddComponent<CanvasGroup>();
        }
        chestUICanvasGroup.alpha = 0;
        chestUICanvasGroup.interactable = false;
        chestUICanvasGroup.blocksRaycasts = false;

        if (nothingHereMessage != null)
        {
            nothingHereMessage.gameObject.SetActive(false);
        }

        chestUI.SetActive(false);
    }

    public void FillChestWithMaterials(MaterialSO[] materials)
    {
        foreach (var material in materials)
        {
            InventorySlot existingSlot = materialsInChest.Find(s => s.Item == material);

            if (existingSlot != null)
            {
                existingSlot.IncreaseQuantity(99);
            }
            else
            {
                materialsInChest.Add(new InventorySlot(material, 99));
            }
        }
    }

    public void ToggleUI()
    {
        if (chestUI.activeSelf)
        {
            CloseUI();
            animator.SetTrigger("Close");
        }
        else
        {
            OpenUI();
            animator.SetTrigger("Open");
        }
    }

    private void OpenUI()
    {
        if (chestUI.activeSelf) return;

        StoreAllMaterials();
        chestUI.SetActive(true);
        StopPlayerMovement();
        DisablePlayerControls();
        PopulateChestUI();
        StartCoroutine(FadeInUI());
    }

    private void CloseUI()
    {
        if (!chestUI.activeSelf) return;

        StartCoroutine(FadeOutUI());
    }

    private IEnumerator FadeInUI()
    {
        float elapsedTime = 0;
        chestUICanvasGroup.interactable = true;
        chestUICanvasGroup.blocksRaycasts = true;

        while (elapsedTime < fadeDuration)
        {
            chestUICanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestUICanvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutUI()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            chestUICanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestUICanvasGroup.alpha = 0;
        chestUI.SetActive(false);
        chestUICanvasGroup.interactable = false;
        chestUICanvasGroup.blocksRaycasts = false;

        EnablePlayerControls();
    }

    public void StoreAllMaterials()
    {
        List<InventorySlot> materialsToStore = playerInventory.RemoveAllMaterials();

        foreach (var slot in materialsToStore)
        {
            if (slot.Item is MaterialSO material)
            {
                InventorySlot existingSlot = materialsInChest.Find(s => s.Item == material);

                if (existingSlot != null)
                {
                    existingSlot.IncreaseQuantity(slot.Quantity ?? 0);
                }
                else
                {
                    materialsInChest.Add(new InventorySlot(material, slot.Quantity));
                }
            }
        }
    }

    private void PopulateChestUI()
    {
        foreach (Transform child in itemGrid)
        {
            Destroy(child.gameObject);
        }

        if (materialsInChest.Count == 0)
        {
            if (nothingHereMessage != null)
            {
                nothingHereMessage.gameObject.SetActive(true);
            }
            return;
        }

        if (nothingHereMessage != null)
        {
            nothingHereMessage.gameObject.SetActive(false);
        }

        foreach (var slot in materialsInChest)
        {
            if (slot.Quantity <= 0)
            {
                Debug.LogWarning($"Material {slot.Item.itemName} has zero quantity. Skipping.");
                continue;
            }

            var itemButton = Instantiate(itemSlotPrefab, itemGrid);

            var icon = itemButton.GetComponentInChildren<Image>();
            var nameAndQuantityText = itemButton.GetComponentInChildren<TextMeshProUGUI>();

            if (icon == null || nameAndQuantityText == null)
            {
                Debug.LogError("Prefab structure is missing Image or TextMeshProUGUI component. Verify the prefab setup.");
                continue;
            }

            icon.sprite = slot.Item.inventoryIcon;
            nameAndQuantityText.text = $"{slot.Item.name}: {slot.Quantity}";
        }
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

    public int GetItemCount(ItemSO item)
    {
        InventorySlot slot = materialsInChest.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    public bool HasMaterials(List<ItemRequirement> requiredMaterials)
    {
        foreach (var requirement in requiredMaterials)
        {
            int playerMaterialCount = GetItemCount(requirement.material);

            if (playerMaterialCount < requirement.quantity)
            {
                Debug.Log($"Insufficient material: {requirement.material.itemName}. Required: {requirement.quantity}, Available: {playerMaterialCount}");
                return false;
            }
        }
        Debug.Log("All required materials are available.");
        return true;
    }

    public void ConsumeMaterials(List<ItemRequirement> requiredMaterials)
    {
        foreach (var requirement in requiredMaterials)
        {
            int quantityToRemove = requirement.quantity;

            InventorySlot slot = materialsInChest.Find(s => s.Item == requirement.material);
            while (quantityToRemove > 0 && slot != null)
            {
                if (slot.Quantity.HasValue && slot.Quantity > quantityToRemove)
                {
                    slot.DecreaseQuantity(quantityToRemove);
                    quantityToRemove = 0;
                }
                else
                {
                    quantityToRemove -= slot.Quantity ?? 0;
                    materialsInChest.Remove(slot);
                    slot = materialsInChest.Find(s => s.Item == requirement.material);
                }
            }
        }

        uiManager.UpdateUI();
        NotifyRecipeUI();
    }

    private void NotifyRecipeUI()
    {
        RecipeUI recipeUI = FindFirstObjectByType<RecipeUI>();
        if (recipeUI != null)
        {
            recipeUI.UpdateUI();
        }
    }
}