using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorSO : ItemSO
{
    public float damageReduction;

    protected override ItemType GetItemType()
    {
        return ItemType.Armor;
    }
}