using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Item Type Headers")]
    // Physical
    [SerializeField] private GameObject Interactable_Header;
    [SerializeField] private GameObject Consumable_Header;
    [SerializeField] private GameObject Physical_Item_Header;

    // Asset
    [SerializeField] private GameObject Oddity_Header;
    [SerializeField] private GameObject Relation_Header;
    [SerializeField] private GameObject Favour_Header;
    [SerializeField] private GameObject Proficiency_Header;
    [SerializeField] private GameObject Event_Header;
    [SerializeField] private GameObject Miscellaneous_Header;

    [Header("Item Type Inventories")]
    // Physical
    [SerializeField] private Transform Interactable_Slots;
    [SerializeField] private Transform Consumable_Slots;
    [SerializeField] private Transform Physical_Item_Slots;

    // Asset
    [SerializeField] private Transform Oddity_Slots;
    [SerializeField] private Transform Relation_Slots;
    [SerializeField] private Transform Favour_Slots;
    [SerializeField] private Transform Proficiency_Slots;
    [SerializeField] private Transform Event_Slots;
    [SerializeField] private Transform Miscellaneous_Slots;

    [Header("Other")]
    [SerializeField] private PlayerManager Player_Manager;
    [SerializeField] private ItemDatabase Item_Database;
    public Inventory Inventory;
    [SerializeField] private GameObject PhysicalInventory;
    [SerializeField] private GameObject AssetInventory;
    [SerializeField] private GameObject InventorySlotPrefab;
    [SerializeField] private GameObject EquipmentSlotPrefab;
    [SerializeField] private TextMeshProUGUI EquipmentInventoryHeader;
    [SerializeField] private GameObject EquipmentInventory;
    public GameObject EquipmentDropdownPanel;
    public string SelectedEquipmentSlot = null;

    private List<GameObject> UIEquipmentSlots = new List<GameObject>();
    private List<UIInventorySlot> UIInventorySlots = new List<UIInventorySlot>();



    public void AddItem(string itemID, int quantity)
    {
        Item item = Item_Database.GetItem(itemID);
        ItemSlot itemSlot = new ItemSlot(item, quantity);
        Inventory.AddItem(itemSlot);
    }

    public void RemoveItem(string itemID, int quantity)
    {
        Item item = Item_Database.GetItem(itemID);
        ItemSlot itemSlot = new ItemSlot(item, quantity);
        Inventory.RemoveItem(itemSlot);
    }

    public void AddEquipment(string itemID)
    {
        Equipment equipment = Item_Database.GetEquipment(itemID);
        ItemSlot itemSlot = new ItemSlot(equipment);
        Inventory.AddItem(itemSlot);
    }

    public void RefreshInventoryUI()
    {

        if (UIInventorySlots.Count > 0)
        {
            foreach (UIInventorySlot UIInventorySlot in UIInventorySlots)
            {
                Destroy(UIInventorySlot.gameObject);
            }
            UIInventorySlots.Clear();
        }

        Interactable_Header.SetActive(false);
        Consumable_Header.SetActive(false);
        Physical_Item_Header.SetActive(false);

        Oddity_Header.SetActive(false);
        Relation_Header.SetActive(false);
        Favour_Header.SetActive(false);
        Proficiency_Header.SetActive(false);
        Event_Header.SetActive(false);
        Miscellaneous_Header.SetActive(false);


        foreach (ItemSlot itemSlot in Inventory.itemSlots)
        {
            GameObject _gameObject;

            if (itemSlot.item != null)
            {
                switch (itemSlot.item.itemType)
                {
                    case "Interactable":
                        _gameObject = Instantiate(InventorySlotPrefab, Interactable_Slots);
                        Interactable_Header.SetActive(true);
                        break;
                    case "Consumable":
                        _gameObject = Instantiate(InventorySlotPrefab, Consumable_Slots);
                        Consumable_Header.SetActive(true);
                        break;
                    case "PhysicalItem":
                        Debug.Log("Creating Item");
                        _gameObject = Instantiate(InventorySlotPrefab, Physical_Item_Slots);
                        Physical_Item_Header.SetActive(true);
                        break;

                    case "Oddity":
                        _gameObject = Instantiate(InventorySlotPrefab, Oddity_Slots);
                        Oddity_Header.SetActive(true);
                        break;
                    case "Relation":
                        _gameObject = Instantiate(InventorySlotPrefab, Relation_Slots);
                        Relation_Header.SetActive(true);
                        break;
                    case "Favour":
                        _gameObject = Instantiate(InventorySlotPrefab, Favour_Slots);
                        Favour_Header.SetActive(true);
                        break;
                    case "Proficiency":
                        _gameObject = Instantiate(InventorySlotPrefab, Proficiency_Slots);
                        Proficiency_Header.SetActive(true);
                        break;
                    case "Event":
                        _gameObject = Instantiate(InventorySlotPrefab, Event_Slots);
                        Event_Header.SetActive(true);
                        break;
                    case "Miscellaneous":
                        _gameObject = Instantiate(InventorySlotPrefab, Miscellaneous_Slots);
                        Miscellaneous_Header.SetActive(true);
                        break;
                    default:
                        _gameObject = Instantiate(InventorySlotPrefab, Physical_Item_Slots);
                        Physical_Item_Header.SetActive(true);
                        break;
                }
                UIInventorySlot _UIInventorySlot = _gameObject.GetComponent<UIInventorySlot>();
                _UIInventorySlot.Tooltip_Trigger.header = itemSlot.item.itemName;
                _UIInventorySlot.Tooltip_Trigger.content = itemSlot.item.itemDescription;
                _UIInventorySlot.quantityTextUI.text = itemSlot.quantity.ToString();
                if (itemSlot.quantity <= 1)
                {
                    _UIInventorySlot.quantityTextUI.text = string.Empty;
                }
                _UIInventorySlot.itemIcon.sprite = itemSlot.item.itemIcon;
                UIInventorySlots.Add(_UIInventorySlot);
            }

        }

    }

    public void ToggleEquipmentDropdown()
    {
        EquipmentDropdownPanel.SetActive(!EquipmentDropdownPanel.activeSelf);
        HideEquipmentMenu();
    }

    public void ShowPhysicalInventory()
    {
        PhysicalInventory.SetActive(true);
        AssetInventory.SetActive(false);
    }

    public void ShowAssetInventory()
    {
        AssetInventory.SetActive(true);
        PhysicalInventory.SetActive(false);
    }

    public void OpenEquipmentMenu()
    {

        if (SelectedEquipmentSlot == "Equipment_Trinket1" || SelectedEquipmentSlot == "Equipment_Trinket2" || SelectedEquipmentSlot == "Equipment_Trinket3")
        {
            EquipmentInventoryHeader.text = "Trinket";

            foreach (GameObject _gameObject in UIEquipmentSlots)
            {
                Destroy(_gameObject);
            }
            UIEquipmentSlots.Clear();

            foreach (ItemSlot itemSlot in Inventory.itemSlots)
            {
                if (itemSlot.equipment != null && itemSlot.equipment.itemType == "Trinket")
                {
                    GameObject _equipmentSlot = Instantiate(EquipmentSlotPrefab, EquipmentInventory.transform);
                    UIEquipmentSlot _UIEquipmentSlot = _equipmentSlot.GetComponent<UIEquipmentSlot>();
                    _UIEquipmentSlot.Player_Manager = Player_Manager;
                    _UIEquipmentSlot.equipment = itemSlot.equipment;
                    _UIEquipmentSlot.Tooltip_Trigger.header = itemSlot.equipment.itemName;
                    _UIEquipmentSlot.Tooltip_Trigger.content = itemSlot.equipment.itemDescription;
                    UIEquipmentSlots.Add(_equipmentSlot);
                }
            }

            EquipmentInventory.SetActive(true);
            return;
        }

        if (SelectedEquipmentSlot == "Equipment_Charm1" || SelectedEquipmentSlot == "Equipment_Charm2" || SelectedEquipmentSlot == "Equipment_Charm3")
        {
            EquipmentInventoryHeader.text = SelectedEquipmentSlot;

            foreach (GameObject _gameObject in UIEquipmentSlots)
            {
                Destroy(_gameObject);
            }
            UIEquipmentSlots.Clear();

            foreach (ItemSlot itemSlot in Inventory.itemSlots)
            {
                if (itemSlot.equipment != null && itemSlot.equipment.itemType == "Charm")
                {
                    GameObject _equipmentSlot = Instantiate(EquipmentSlotPrefab, EquipmentInventory.transform);
                    UIEquipmentSlot _UIEquipmentSlot = _equipmentSlot.GetComponent<UIEquipmentSlot>();
                    _UIEquipmentSlot.Player_Manager = Player_Manager;
                    _UIEquipmentSlot.equipment = itemSlot.equipment;
                    _UIEquipmentSlot.Tooltip_Trigger.header = itemSlot.equipment.itemName;
                    _UIEquipmentSlot.Tooltip_Trigger.content = itemSlot.equipment.itemDescription;
                    UIEquipmentSlots.Add(_equipmentSlot);
                }
            }

            EquipmentInventory.SetActive(true);
            return;
        }


        EquipmentInventoryHeader.text = SelectedEquipmentSlot;

        foreach (GameObject _gameObject in UIEquipmentSlots)
        {
            Destroy(_gameObject);
        }
        UIEquipmentSlots.Clear();

        foreach (ItemSlot itemSlot in Inventory.itemSlots)
        {
            if (itemSlot.equipment != null && itemSlot.equipment.itemType == SelectedEquipmentSlot)
            {
                GameObject _equipmentSlot = Instantiate(EquipmentSlotPrefab,EquipmentInventory.transform);
                UIEquipmentSlot _UIEquipmentSlot = _equipmentSlot.GetComponent<UIEquipmentSlot>();
                _UIEquipmentSlot.Player_Manager = Player_Manager;
                _UIEquipmentSlot.equipment = itemSlot.equipment;
                _UIEquipmentSlot.Tooltip_Trigger.header = itemSlot.equipment.itemName;
                _UIEquipmentSlot.Tooltip_Trigger.content = itemSlot.equipment.itemDescription;
                UIEquipmentSlots.Add(_equipmentSlot);
            }
        }

        EquipmentInventory.SetActive(true);
    }

    public void HideEquipmentMenu()
    {
        EquipmentInventory.SetActive(false);
    }



    public void ShowEquipment(string equipmentType)
    {
        SelectedEquipmentSlot = equipmentType;
        OpenEquipmentMenu();
    }

    // 1: Equipment_Hat
    // 2: Equipment_Face
    // 3: Equipment_Eye
    // 4: Equipment_Mouth
    // 5: Equipment_Glove
    // 6: Equipment_Boot
    // 7: Equipment_Body
    // 8: Equipment_OuterBody
    // 9: Equipment_Back
    // 10: Equipment_Charm1
    // 11: Equipment_Charm2
    // 12: Equipment_Charm3

    // 13: Equipment_Weapon

    // 14: Equipment_Trinket1
    // 15: Equipment_Trinket2
    // 16: Equipment_Trinket3
}
