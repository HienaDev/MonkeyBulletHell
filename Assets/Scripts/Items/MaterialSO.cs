using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialSO : ItemSO
{
    public int maxStackSize;

    private void Awake()
    {
        itemType = ItemType.Material;
    }
}

