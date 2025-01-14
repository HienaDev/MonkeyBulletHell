using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialSO : ItemSO
{

    public AudioClip[] pickupSounds;

        protected override ItemType GetItemType()
    {
        return ItemType.Material;
    }
}