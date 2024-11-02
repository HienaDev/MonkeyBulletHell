using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    [HideInInspector] public ItemType itemType;
    public Sprite inventoryIcon;
}
