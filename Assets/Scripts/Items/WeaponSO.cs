using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    public float speedWithWeapon;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float shotSpeed = 100f;
    public bool laserLikeProjectile = false;
    public float laserDuration = 0f;

    private void Awake()
    {
        itemType = ItemType.Weapon;
    }
}