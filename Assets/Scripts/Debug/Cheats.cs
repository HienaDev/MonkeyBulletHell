using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private Chest chest;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private MaterialSO[] materials;
    [SerializeField] private ToolSO pickaxe;
    [SerializeField] private ToolSO axe;
    [SerializeField] private HealthSystem moaiHealth;

    void Update()
    {   
        // Store all materials in chest
        if (Input.GetKeyDown(KeyCode.F1))
        {
            chest.StoreAllMaterials();
        }

        // Fill chest with materials
        if (Input.GetKeyDown(KeyCode.F2))
        {
            chest.FillChestWithMaterials(materials);
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
    }
}
