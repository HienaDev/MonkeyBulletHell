public class InventorySlot
{
    public ItemSO Item { get; private set; }
    public int? Quantity { get; private set; }

    public InventorySlot(ItemSO item, int? quantity = null)
    {
        Item = item;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (Quantity.HasValue)
            Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (Quantity.HasValue && Quantity > amount)
            Quantity -= amount;
        else
            Quantity = null;
    }
}