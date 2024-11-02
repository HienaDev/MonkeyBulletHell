using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class ArmorSO : ItemSO
{
    public float damageReduction;

    //TO DO: public SpecialAbility specialAbility;

    private void Awake()
    {
        itemType = ItemType.Armor;
    }
}