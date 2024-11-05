using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemSO result;
    public List<ItemRequirement> requiredMaterials;
    public bool IsUnlocked { get; private set; }
    public ItemType ItemType => result.itemType;

    public void Unlock()
    {
        IsUnlocked = true;
    }

    public bool isAlreadyCrafted = false;
}

[System.Serializable]
public class ItemRequirement
{
    public ItemSO material;
    public int quantity;
}