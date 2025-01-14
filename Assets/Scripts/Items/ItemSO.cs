using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite inventoryIcon;
    public GameObject itemPrefab;

    [SerializeField, HideInInspector]
    private ItemType itemType;

    public ItemType ItemType => itemType;

    protected abstract ItemType GetItemType();

    protected virtual void Reset()
    {
        itemType = GetItemType();
    }
}