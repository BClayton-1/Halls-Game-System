using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();



    public void AddItem(ItemSlot itemSlot)
    {
        if (itemSlot.equipment != null)
        {
            for (int i = itemSlots.Count - 1; i >= 0; i--)
            {
                if (itemSlots[i].equipment != null && itemSlots[i].equipment.itemID == itemSlot.equipment.itemID)
                {
                    return;
                }
            }
            itemSlots.Add(new ItemSlot(itemSlot.equipment));
            return;
        }

        for (int i = itemSlots.Count - 1; i >= 0; i--)
        {
            if (itemSlots[i].item != null && itemSlots[i].item.itemID == itemSlot.item.itemID)
            {
                itemSlots[i] = new ItemSlot(itemSlots[i].item, itemSlots[i].quantity + itemSlot.quantity);

                // Update item slot

                return;
            }
        }
        itemSlots.Add(new ItemSlot(itemSlot.item, itemSlot.quantity));
        // Update item slot
    }

    public void RemoveItem(ItemSlot itemSlot)
    {
        for (int i = itemSlots.Count - 1; i >= 0; i--)
        {
            if (itemSlots[i].item == null)
            {
                continue;
            }

            if (itemSlots[i].item.itemID == itemSlot.item.itemID)
            {
                if (itemSlots[i].quantity < itemSlot.quantity)
                {
                    itemSlots[i] = new ItemSlot(itemSlots[i].item,0);

                    itemSlots[i] = new ItemSlot();

                }
                else
                {
                    itemSlots[i] = new ItemSlot(itemSlots[i].item, itemSlots[i].quantity - itemSlot.quantity);
                    if (itemSlots[i].quantity == 0)
                    {
                        itemSlots.Remove(itemSlots[i]);

                    }
                }

                // Update item slot

                return;
            }
        }
    }

    public bool HasItem(Item item)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item.itemID == item.itemID)
            {
                return true;
            }
        }

        return false;
    }

    public int GetTotalQuantity(string itemID)
    {
        int totalCount = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item != null && itemSlot.item.itemID == itemID)
            {
                totalCount = itemSlot.quantity;
                return totalCount;
            }
        }

        return totalCount;
    }
}
