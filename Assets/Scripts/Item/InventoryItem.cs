using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Item
{

    public InventoryItem(string _itemID, string _itemName, string _itemDescription,Sprite _itemIcon, string _itemType = "PhysicalItem")
    {
        itemType = _itemType;
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemIcon = _itemIcon;
    }


}
