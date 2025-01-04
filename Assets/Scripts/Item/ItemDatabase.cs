using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> itemDataBase = new List<Item>();
    private List<Equipment> equipmentDataBase = new List<Equipment>();

    void Awake()
    {
        BuildItemDatabase();
        BuildEquipmentDatabase();
    }

    public Item GetItem(string itemID)
    {
        return itemDataBase.Find(item => item.itemID == itemID);
    }

    public Equipment GetEquipment(string itemID)
    {
        return equipmentDataBase.Find(equipment => equipment.itemID == itemID);
    }

    void BuildItemDatabase()
    {
        itemDataBase = new List<Item>()
        {
            // Interactables

            // Consumables

            // Physical Items
            new InventoryItem
            (
                /*Item ID*/ "Bones",
                /*Name*/ "Bones",
                /*Description*/ "Bones are for burying!",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Bones"),
                /*Item Type*/ "PhysicalItem"
            ),
            new InventoryItem
            (
                /*Item ID*/ "Cheese",
                /*Name*/ "Cheese",
                /*Description*/ "Loved by rats",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Cheese"),
                /*Item Type*/ "PhysicalItem"
            ),



            // Oddity
            // Relation
            new InventoryItem
            (
                /*Item ID*/ "Relation_Rats",
                /*Name*/ "Relation: The Rat Kingdom",
                /*Description*/ "Your relationship with the Rat Kingdom.",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Bones"),
                /*Item Type*/ "Relation"
            ),
            // Favour
            // Proficiency
            // Event
			new InventoryItem
            (
                /*Item ID*/ "Event_Learning_About_Halls",
                /*Name*/ "Learning about The Halls",
                /*Description*/ "Just what kind of place is this?.",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Bones"),
                /*Item Type*/ "Event"
            )
            // Miscellaneous

        };
    }



    void BuildEquipmentDatabase()
    {
        equipmentDataBase = new List<Equipment>()
        {

            // Equipment_Headgear
            // Equipment_Hat
            // Equipment_Face
            // Equipment_Eye
            new Equipment
            (
                /*Item ID*/ "Eyepatch",
                /*Name*/ "Eyepatch",
                /*Description*/ "Makes you look like a badass- or a pirate. Maybe even a badass pirate!",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Cheese"),
                /*Item Type*/ "Equipment_Eye",
                /*Tenacity Modifier*/ -1,
                /*Cognition Modifier*/ 0,
                /*Influence Modifier*/ 3,
                /*Luck Modifier*/ 0,
                /*Armor*/ 0
            ),
            // Equipment_Mouth
            // Equipment_Glove
            // Equipment_Boot
            // Equipment_Body
            // Equipment_OuterBody
            // Equipment_Back
            // Equipment_Charm1
            // Equipment_Charm2
            // Equipment_Charm3

            // 13: Equipment_Weapon

            // 14: Trinket
            new Equipment
            (
                /*Item ID*/ "Stinky_Cheese",
                /*Name*/ "Stinky Cheese",
                /*Description*/ "Holding this cheese makes you smell awful, but the rats seem to love it.",
                /*Sprite*/ Resources.Load<Sprite>("Images/UI/Icon/Item/Cheese"),
                /*Item Type*/ "Trinket",
                /*Tenacity Modifier*/ 0,
                /*Cognition Modifier*/ 0,
                /*Influence Modifier*/ -5,
                /*Luck Modifier*/ 0,
                /*Armor*/ 0
            )

        };
    }

}
