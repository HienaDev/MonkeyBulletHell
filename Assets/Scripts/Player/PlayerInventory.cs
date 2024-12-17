using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int maxInventoryMaterialsAndToolsSlots = 6;
    [SerializeField] private Transform armorAnchor;

    private ItemSO[] weaponSlots;
    private List<InventorySlot> inventoryItems;
    private List<CraftingRecipe> alreadyCraftedRecipes;
    private ShootingPlayer shootingPlayerScript;
    private ItemSO equippedArmor;
    private GameObject currentArmorModel;

    private Tutorial tutorial;

    private void Start()
    {

        tutorial = Tutorial.Instance;

        weaponSlots = new ItemSO[2];
        inventoryItems = new List<InventorySlot>();
        alreadyCraftedRecipes = new List<CraftingRecipe>();

        shootingPlayerScript = FindFirstObjectByType<ShootingPlayer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropSelectedItem();
        }
    }

    public void AddItem(ItemSO item)
    {
        if (item.itemType == ItemType.Material)
        {
            tutorial.UpdateCollectTasks(item as MaterialSO);
            InventorySlot existingSlot = inventoryItems.Find(slot => slot.Item == item && slot.Quantity.HasValue);

            if (existingSlot != null)
            {
                existingSlot.IncreaseQuantity(1);
                Debug.Log($"{item.itemName} quantity increased to {existingSlot.Quantity}");
                uiManager.UpdateUI();
                return;
            }
        }

        if (item.itemType == ItemType.Weapon)
        {
  
            if (weaponSlots[0] == null)
            {
                weaponSlots[0] = item;
                Debug.Log($"{item.itemName} added to weapon slot 1");
            }
            else if (weaponSlots[1] == null)
            {
                weaponSlots[1] = item;
                Debug.Log($"{item.itemName} added to weapon slot 2");
            }
            else
            {
                Debug.LogWarning("Both weapon slots are full.");
            }
        }
        else
        {
            if (inventoryItems.Count < maxInventoryMaterialsAndToolsSlots)
            {
                int? quantity = item.itemType == ItemType.Material ? 1 : (int?)null;
                inventoryItems.Add(new InventorySlot(item, quantity));
                Debug.Log($"{item.itemName} added to inventory.");
            }
            else
            {
                Debug.LogWarning("No more inventory slots available.");
            }
        }

        uiManager.UpdateUI();
    }

    public void AddCraftedRecipe(CraftingRecipe recipe)
    {
        if (!alreadyCraftedRecipes.Contains(recipe))
        {
            alreadyCraftedRecipes.Add(recipe);
            Debug.Log($"{recipe.result.itemName} recipe added to the crafted list.");
        }
    }

    public void RemoveCraftedRecipe(CraftingRecipe recipe)
    {
        if (alreadyCraftedRecipes.Contains(recipe))
        {
            alreadyCraftedRecipes.Remove(recipe);
            Debug.Log($"{recipe.result.itemName} recipe removed from the crafted list.");
        }
        else
        {
            Debug.LogWarning($"{recipe.result.itemName} recipe is not in the crafted list.");
        }
    }

    public bool IsRecipeCrafted(CraftingRecipe recipe)
    {
        return alreadyCraftedRecipes.Contains(recipe);
    }

    public void GetCraftedRecipes(out List<CraftingRecipe> recipes)
    {
        recipes = new List<CraftingRecipe>(alreadyCraftedRecipes);
    }

    public void RemoveItem(ItemSO item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            if (weaponSlots[0] == item)
            {
                weaponSlots[0] = weaponSlots[1];
                weaponSlots[1] = null;

                Debug.Log($"{item.itemName} removed from weapon slot 1. Slot 2 weapon moved to slot 1.");
            }
            else if (weaponSlots[1] == item)
            {
                weaponSlots[1] = null;
                Debug.Log($"{item.itemName} removed from weapon slot 2.");
            }
        }
        else
        {
            InventorySlot slot = inventoryItems.Find(s => s.Item == item);
            if (slot != null)
            {
                inventoryItems.Remove(slot);
                Debug.Log($"{item.itemName} removed from inventory.");
            }
            else
            {
                Debug.LogWarning($"{item.itemName} not found in inventory.");
            }
        }

        uiManager.UpdateUI();
        NotifyRecipeUI();
    }

    public List<InventorySlot> RemoveAllMaterials()
    {
        List<InventorySlot> materialsToStore = new List<InventorySlot>();

        foreach (var slot in inventoryItems)
        {
            if (slot.Item.itemType == ItemType.Material)
            {
                materialsToStore.Add(slot);
            }
        }

        inventoryItems.RemoveAll(slot => slot.Item.itemType == ItemType.Material);

        uiManager.UpdateUI();

        return materialsToStore;
    }

    public bool MaterialAndToolSlotsFull()
    {
        return inventoryItems.Count >= maxInventoryMaterialsAndToolsSlots;
    }

    public void DropSelectedItem()
    {
        int selectedSlotIndex = uiManager.GetSelectedSlotIndex();
        if (selectedSlotIndex == -1)
        {
            Debug.Log("No item selected or invalid slot.");
            return;
        }

        ItemSO selectedItem = GetItemAtSlot(selectedSlotIndex);
        if (selectedItem == null || selectedItem.itemType == ItemType.Weapon)
        {
            Debug.Log("Cannot drop weapon or no item found in selected slot.");
            return;
        }

        int lastSelectedIndex = selectedSlotIndex;

        Vector3 dropPosition = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(dropPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Floor")))
        {
            dropPosition.y = hit.point.y;
        }
        else
        {
            Debug.LogWarning("Floor not found! Using default Y position.");
        }

        if (selectedItem.itemPrefab != null)
        {
            Quaternion dropRotation = Quaternion.Euler(selectedItem.itemPrefab.transform.rotation.eulerAngles);
            Instantiate(selectedItem.itemPrefab, dropPosition, dropRotation);
        }
        else
        {
            Debug.LogWarning("No prefab assigned for this item.");
        }

        RemoveItem(selectedItem);

        uiManager.UpdateUI();

        ItemSO currentItem = GetItemAtSlot(lastSelectedIndex);
        if (currentItem != null && currentItem.itemType == ItemType.Material && GetItemCount(currentItem) > 0)
        {
            uiManager.SelectInventorySlot(lastSelectedIndex);
        }
        else
        {
            uiManager.SelectClosestAvailableSlot(lastSelectedIndex);
        }
    }

    public void EquipWeapon(int slot, ItemSO weapon)
    {
        if (weapon.itemType != ItemType.Weapon)
        {
            Debug.LogWarning("Cannot equip a non-weapon item in a weapon slot.");
            return;
        }

        if (slot == 2 && weaponSlots[0] == null)
        {
            Debug.LogWarning("Cannot equip weapon in slot 2 if slot 1 is empty.");
            return;
        }

        if (slot == 1)
        {
            weaponSlots[0] = weapon;
            Debug.Log($"{weapon.itemName} equipped in slot 1.");
        }
        else if (slot == 2)
        {
            weaponSlots[1] = weapon;
            Debug.Log($"{weapon.itemName} equipped in slot 2.");
        }

        uiManager.UpdateUI();
        NotifyRecipeUI();
    }

    public void EquipArmor(ItemSO armor)
    {
        if (armor.itemType != ItemType.Armor)
        {
            Debug.LogWarning("Cannot equip a non-armor item as armor.");
            return;
        }

        equippedArmor = armor;
        Debug.Log($"{armor.itemName} equipped as armor.");

        if (currentArmorModel != null)
        {
            Destroy(currentArmorModel);
        }

        if (armor.itemPrefab != null)
        {
            currentArmorModel = Instantiate(armor.itemPrefab, armorAnchor);
            currentArmorModel.transform.localPosition = Vector3.zero;
            currentArmorModel.transform.localRotation = Quaternion.identity;
            Debug.Log("Armor model equipped.");
        }
        else
        {
            Debug.LogWarning("This armor does not have a prefab.");
        }

        uiManager.UpdateUI();
        NotifyRecipeUI();
    }

    public void UnequipArmor()
    {
        if (equippedArmor != null)
        {
            Debug.Log($"{equippedArmor.itemName} unequipped as armor.");

            if (currentArmorModel != null)
            {
                Destroy(currentArmorModel);
                currentArmorModel = null;
            }

            equippedArmor = null;

            uiManager.UpdateUI();
            NotifyRecipeUI();
        }
        else
        {
            Debug.LogWarning("No armor is currently equipped to unequip.");
        }
    }


    private void NotifyRecipeUI()
    {
        RecipeUI recipeUI = FindFirstObjectByType<RecipeUI>();
        if (recipeUI != null)
        {
            recipeUI.UpdateUI();
        }
    }

    public bool IsWeaponEquippedInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] != null : weaponSlots[1] != null;
    }

    public bool IsWeaponEquippedInSlot(int slot, ItemSO weapon)
    {
        return slot == 1 ? weaponSlots[0] == weapon : weaponSlots[1] == weapon;
    }

    public bool PlayerHasWeaponEquipped()
    {

        return IsWeaponEquippedInSlot(0) || IsWeaponEquippedInSlot(1); 
    }

    public bool IsSlotOccupied(int slot, ItemType itemType)
    {
        if (slot == 1) return weaponSlots[0] != null && weaponSlots[0].itemType == itemType;
        if (slot == 2) return weaponSlots[1] != null && weaponSlots[1].itemType == itemType;
        return false;
    }

    public int GetItemCount(ItemSO item)
    {
        InventorySlot slot = inventoryItems.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    public ItemSO GetWeaponInSlot(int slot)
    {
        return slot == 1 ? weaponSlots[0] : weaponSlots[1];
    }

    public void EquipWeapon(int slot)
    {
        Debug.Log("try equip weapon");
        if(weaponSlots[slot] is WeaponSO)
            shootingPlayerScript.SetWeapon((weaponSlots[slot] as WeaponSO));
    }

    public ItemSO GetItemAtSlot(int slot)
    {
        if (slot < 0) return null;

        if (slot < 2)
        {
            return weaponSlots[slot];
        }

        int adjustedSlot = slot - 2;
        if (adjustedSlot >= 0 && adjustedSlot < inventoryItems.Count)
        {
            return inventoryItems[adjustedSlot].Item;
        }

        return null;
    }

    public List<InventorySlot> GetInventoryItems()
    {
        return inventoryItems;
    }

    public ItemSO GetSelectedItem()
    {
        int selectedSlotIndex = uiManager.GetSelectedSlotIndex();
        return GetItemAtSlot(selectedSlotIndex);
    }

    public bool IsArmorEquipped(ItemSO armor)
    {
        return equippedArmor == armor;
    }

    public ArmorSO GetEquippedArmor()
    {
        return equippedArmor as ArmorSO;
    }

    public bool ContainsMaterial(MaterialSO material)
    {
        return inventoryItems.Exists(slot => slot.Item == material);
    }

    public void ClearInventoryOnDeath()
    {
        RemoveEquipmentFromCraftedRecipes();

        if (inventoryItems != null)
        {
            inventoryItems.Clear();
        }

        if (weaponSlots != null)
        {
            weaponSlots[0] = null;
            weaponSlots[1] = null;
        }

        equippedArmor = null;

        if (currentArmorModel != null)
        {
            Destroy(currentArmorModel);
            currentArmorModel = null;
        }

        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }
    }

    private void RemoveEquipmentFromCraftedRecipes()
    {
        if (alreadyCraftedRecipes == null)
        {
            return;
        }

        foreach (var weapon in weaponSlots)
        {
            if (weapon != null)
            {
                CraftingRecipe recipe = alreadyCraftedRecipes.Find(r => r.result == weapon);
                if (recipe != null)
                {
                    alreadyCraftedRecipes.Remove(recipe);
                }
            }
        }

        foreach (var slot in inventoryItems)
        {
            if (slot.Item.itemType == ItemType.Tool)
            {
                CraftingRecipe recipe = alreadyCraftedRecipes.Find(r => r.result == slot.Item);
                if (recipe != null)
                {
                    alreadyCraftedRecipes.Remove(recipe);
                }
            }
        }

        if (equippedArmor != null)
        {
            CraftingRecipe armorRecipe = alreadyCraftedRecipes.Find(r => r.result == equippedArmor);
            if (armorRecipe != null)
            {
                alreadyCraftedRecipes.Remove(armorRecipe);
            }
        }
    }

    public bool IsToolEquipped(ItemSO tool)
    {
        return inventoryItems.Find(slot => slot.Item == tool) != null;
    }
}