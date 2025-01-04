public struct ItemSlot
{
    public Item item;
    public Equipment equipment;
    public int quantity;

    public ItemSlot(Item _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;

        equipment = null;
    }

    public ItemSlot(Equipment _equipment)
    {
        item = null;
        quantity = 1;

        equipment = _equipment;
    }

}
