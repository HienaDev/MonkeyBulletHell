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
        if (Input.GetKeyDown(KeyCode.U))
        {
            chest.StoreAllMaterials();
        }

        // Fill chest with materials
        if (Input.GetKeyDown(KeyCode.I))
        {
            chest.FillChestWithMaterials(materials);
        }

        // Add Pickaxe to player inventory
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerInventory.AddItem(pickaxe);
        }

        // Add Axe to player inventory
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerInventory.AddItem(axe);
        }
    }
}
