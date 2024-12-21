[System.Serializable]
public class InventorySlot
{
    public ItemSO Item;
    public int Quantity;

    public InventorySlot(ItemSO item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        Quantity -= amount;

        if (Quantity < 0)
            Quantity = 0;
    }
}