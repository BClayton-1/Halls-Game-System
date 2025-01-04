using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public bool equipped = false;

    public int tenacityModifier = 0;
    public int cognitionModifier = 0;
    public int influenceModifier = 0;
    public int luckModifier = 0;

    public int armor = 0;


    public Equipment(string _itemID, string _itemName, string _itemDescription, Sprite _itemIcon, string _itemType = "Trinket", int _tenacityModifier = 0, int _cognitionModifier = 0, int _influenceModifier = 0, int _luckModifier = 0, int _armor = 0)
    {
        itemType = _itemType;
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemIcon = _itemIcon;

        tenacityModifier = _tenacityModifier;
        cognitionModifier = _cognitionModifier;
        influenceModifier = _influenceModifier;
        luckModifier = _luckModifier;

        armor = _armor;
    }

}
