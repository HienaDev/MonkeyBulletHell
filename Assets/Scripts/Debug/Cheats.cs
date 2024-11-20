using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private Chest chest;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private MaterialSO[] materials;
    [SerializeField] private ToolSO pickaxe;
    [SerializeField] private ToolSO axe;

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
    }
}
