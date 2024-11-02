using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialSO : ItemSO
{
    private void Awake()
    {
        itemType = ItemType.Material;
    }
}

