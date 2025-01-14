using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialSO : ItemSO
{
    protected override ItemType GetItemType()
    {
        return ItemType.Material;
    }
}