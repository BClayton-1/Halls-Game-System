using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    [Header("Basic Info")]
    public string itemID = "defaultitem";
    public string itemName = "Default Item";
    public string itemDescription = "This is the default item";
    public Sprite itemIcon = null;
    public string itemType = "Default";
}
