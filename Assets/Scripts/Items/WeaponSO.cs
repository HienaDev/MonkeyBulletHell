using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    public float speedWithWeapon;
    public GameObject projectilePrefab;

    private void Awake()
    {
        itemType = ItemType.Weapon;
    }
}