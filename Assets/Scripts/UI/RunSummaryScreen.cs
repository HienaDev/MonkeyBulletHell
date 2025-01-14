using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RunSummaryScreen : MonoBehaviour
{
    [SerializeField] private GameObject summaryUI;
    [SerializeField] private Transform itemGrid;
    [SerializeField] private GameObject summaryItemPrefab;
    [SerializeField] private TextMeshProUGUI summaryMessage;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Chest chest;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float itemFadeDuration = 0.5f;
    [SerializeField] private float itemDelay = 0.2f;
    [SerializeField] private float uiFadeOutDuration = 1f;

    private List<GameObject> instantiatedItems = new List<GameObject>();
    private List<InventorySlot> collectedMaterials = new List<InventorySlot>();

    private void Start()
    {
        summaryUI.SetActive(false);
    }

    public void DisplaySummary()
    {
        collectedMaterials = new List<InventorySlot>(playerInventory.GetInventory());

        if (collectedMaterials.Count == 0)
        {
            Debug.Log("No materials to display in the summary.");
            return;
        }

        chest.StoreAllMaterials();

        summaryMessage.gameObject.SetActive(true);

        ClearItemGrid();

        summaryMessage.transform.SetParent(itemGrid, false);
        summaryMessage.transform.SetAsFirstSibling();

        foreach (var slot in collectedMaterials)
        {
            if (slot.Quantity <= 0)
            {
                Debug.LogWarning($"Material {slot.Item.itemName} has zero quantity. Skipping.");
                continue;
            }

            var itemButton = Instantiate(summaryItemPrefab, itemGrid);
            instantiatedItems.Add(itemButton);

            var icon = itemButton.transform.Find("Icon").GetComponent<Image>();
            var quantityText = itemButton.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            if (icon == null || quantityText == null)
            {
                Debug.LogError("Prefab structure is missing 'Icon' or 'Quantity' components. Verify the prefab setup.");
                continue;
            }

            icon.sprite = slot.Item.inventoryIcon;
            quantityText.text = $"{slot.Item.name} x{slot.Quantity}";

            var canvasGroup = itemButton.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = itemButton.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;
        }

        summaryUI.SetActive(true);

        StartCoroutine(FadeInItems());
    }

    private IEnumerator FadeInItems()
    {
        foreach (var item in instantiatedItems)
        {
            if (item == null) continue;

            var canvasGroup = item.GetComponent<CanvasGroup>();
            if (canvasGroup == null) continue;

            float elapsedTime = 0f;

            while (elapsedTime < itemFadeDuration)
            {
                if (canvasGroup == null) yield break;
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / itemFadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(itemDelay);
        }

        StartCoroutine(FadeOutUIAfterDelay());
    }

    private IEnumerator FadeOutUIAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        StartCoroutine(FadeOutUI());
    }

    private IEnumerator FadeOutUI()
    {
        float elapsedTime = 0f;
        CanvasGroup summaryUICanvasGroup = summaryUI.GetComponent<CanvasGroup>();

        if (summaryUICanvasGroup == null)
        {
            summaryUICanvasGroup = summaryUI.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < uiFadeOutDuration)
        {
            summaryUICanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / uiFadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        summaryUICanvasGroup.alpha = 1;
        summaryUICanvasGroup.interactable = false;
        summaryUICanvasGroup.blocksRaycasts = false;
        summaryUI.SetActive(false);
    }

    private void ClearItemGrid()
    {
        foreach (var item in instantiatedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        instantiatedItems.Clear();

        foreach (Transform child in itemGrid)
        {
            if (child != summaryMessage.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}