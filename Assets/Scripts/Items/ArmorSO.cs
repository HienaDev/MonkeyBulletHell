using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorSO : ItemSO
{
    public float damageReduction;

    //public SpecialAbility specialAbility;

    private void Awake()
    {
        itemType = ItemType.Armor;
    }
}