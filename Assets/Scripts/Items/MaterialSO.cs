using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Items/Material")]
public class MaterialSO : ItemSO
{

    public AudioClip[] pickupSounds;

    private void Awake()
    {
        itemType = ItemType.Material;
    }
}

