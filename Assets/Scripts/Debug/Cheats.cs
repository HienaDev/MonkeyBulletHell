using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private Chest chest;
    [SerializeField] private WeaponSO slingShot;
    [SerializeField] private WeaponSO moaiCannon;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private MaterialSO[] materials;
    [SerializeField] private ToolSO pickaxe;
    [SerializeField] private ToolSO axe;
    [SerializeField] private HealthSystem moaiHealth;
    [SerializeField] private RunSummaryScreen runSummaryScreen;
    [SerializeField] private Transform bossStartPosition;
    [SerializeField] private GameObject player;

    void Update()
    {
        // Put boss on phase 2
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            moaiHealth.Heal(50);
            moaiHealth.DealDamage(16);
        }
        // Put boss on phase 3
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            moaiHealth.Heal(50);
            moaiHealth.DealDamage(31);
        }
        // Put boss on enrage phase
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            moaiHealth.Heal(50);
            moaiHealth.DealDamage(46);
        }
        // Put player next to boss
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            player.transform.position = bossStartPosition.position;
        }

        // Store all materials in chest
        if (Input.GetKeyDown(KeyCode.F1))
        {
            chest.StoreAllMaterials();
        }

        // Add slingShot to player inventory
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F1))
        {
            playerInventory.AddItem(slingShot);
        }

        // Fill chest with materials
        if (Input.GetKeyDown(KeyCode.F2))
        {
            chest.FillChestWithMaterials(materials);
        }

        // Add moaiCannon to player inventory
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F2))
        {
            playerInventory.AddItem(moaiCannon);
        }

        // Add Pickaxe to player inventory
        if (Input.GetKeyDown(KeyCode.F3))
        {
            playerInventory.AddItem(pickaxe);
        }

        // Add Axe to player inventory
        if (Input.GetKeyDown(KeyCode.F4))
        {
            playerInventory.AddItem(axe);
        }

        // Clear inventory
        if (Input.GetKeyDown(KeyCode.F5))
        {
            playerInventory.ClearInventoryOnDeath();
        }

        // Kill player
        if (Input.GetKeyDown(KeyCode.F6))
        {
            playerHealthSystem.DealDamage(10000);
        }

        // Heal player
        if (Input.GetKeyDown(KeyCode.F7))
        {
            playerHealthSystem.Heal(10000);
        }

        // Imortal player
        if (Input.GetKeyDown(KeyCode.F8))
        {
            playerHealthSystem.SetImmortal(true);
        }

        // Mortal player
        if (Input.GetKeyDown(KeyCode.F9))
        {
            playerHealthSystem.SetImmortal(false);
        }

        // Deal dmg to player
        if (Input.GetKeyDown(KeyCode.F10))
        {
            playerHealthSystem.DealDamage(1);
        }

        // Kill Boss
        if (Input.GetKeyDown(KeyCode.F11))
        {
            moaiHealth.SetImmortal(false);
            moaiHealth.DealDamage(1000);
        }

        // Display Run Summary Screen
        if (Input.GetKeyDown(KeyCode.F12))
        {
            runSummaryScreen.DisplaySummary();
        }
    }
}
