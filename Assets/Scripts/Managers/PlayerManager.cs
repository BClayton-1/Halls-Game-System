using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InventoryUIManager Inventory_UI_Manager;
    [SerializeField] private GameObject playerMenu_Main;
    [SerializeField] private GameObject playerMenu_Inventory;

    public RectTransform FleshHPUI;
    public RectTransform TempDamageUI;
    public RectTransform PermDamageUI;

    [SerializeField] private TextMeshProUGUI tenText;
    [SerializeField] private TextMeshProUGUI cogText;
    [SerializeField] private TextMeshProUGUI infText;


    [Header("Stats:")]
    public int tenacityStat = 0;
    public int cognitionStat = 0;
    public int influenceStat = 0;
    public int luckStat = 0;

    private int tenacityFromEquipment = 0;
    private int cognitionFromEquipment = 0;
    private int influenceFromEquipment = 0;
    private int luckFromEquipment = 0;

    public int naturalTenacity = 0;
    public int naturalCognition = 0;
    public int naturalInfluence = 0;
    public int naturalLuck = 0;

    [Header("Health:")]
    public float maxHP;
    public float fleshHP;
    public float damageTemporary;
    public float damagePermanent;
    public float healthiness = 1f;

    public int armor = 0;

    [Header("Sanity:")]
    public int composure; // Short-term sanity
    public int sanity; // Long-term sanity

    [Header("Equipment Slots:")]
    public Equipment[] EquipmentSlots = new Equipment[17];
    public Image[] EquipmentSlotVisuals = new Image[17];
    // 0: Equipment_Headgear
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




    // Start is called before the first frame update
    void Start()
    {
        SetUI();
    }

    public void CheckMenuFreezePM()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "2DEnvironment")
        {
            MenuFreeze _MenuFreeze = GameObject.Find("FirstPersonPlayer").GetComponent<MenuFreeze>();
            if (_MenuFreeze != null)
            {
                _MenuFreeze.CheckFreezeMenu();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            TooltipSystem.Hide();
            if (playerMenu_Inventory.activeSelf == true || playerMenu_Main.activeSelf == true)
            {
                playerMenu_Inventory.SetActive(false);
                playerMenu_Main.SetActive(false);
                CheckMenuFreezePM();
                return;
            }

            SetUI();
            playerMenu_Inventory.SetActive(false);
            playerMenu_Main.SetActive(true);
            CheckMenuFreezePM();
        }
        else if (Input.GetKeyDown(KeyCode.I) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            TooltipSystem.Hide();
            if (playerMenu_Inventory.activeSelf == true)
            {
                playerMenu_Inventory.SetActive(false);
                CheckMenuFreezePM();
                return;
            }
            Debug.Log(healthiness);
            playerMenu_Main.SetActive(false);

            Inventory_UI_Manager.RefreshInventoryUI();
            Inventory_UI_Manager.EquipmentDropdownPanel.SetActive(false);
            Inventory_UI_Manager.HideEquipmentMenu();
            Inventory_UI_Manager.ShowPhysicalInventory();
            playerMenu_Inventory.SetActive(true);
            CheckMenuFreezePM();
        }
    }

    public bool IsEquipped(string itemID)
    {
        for (int i = EquipmentSlots.Length - 1; i >= 0; i--)
        {
            if (EquipmentSlots[i] != null && EquipmentSlots[i].itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }

    public void EquipItem(Equipment equipment)
    {
        for (int h = EquipmentSlots.Length - 1; h >= 0; h--)
        {
            if (EquipmentSlots[h] == equipment)
            {
                EquipmentSlots[h] = null;
                EquipmentSlotVisuals[h].sprite = Resources.Load<Sprite>("Images/UI/Blank");
            }
        }

        int i = 0;
        switch (Inventory_UI_Manager.SelectedEquipmentSlot)
        {
            case "Equipment_Headgear":
                i = 0;
                break;

            case "Equipment_Hat":
                i = 1;
                break;

            case "Equipment_Face":
                i = 2;
                break;

            case "Equipment_Eye":
                i = 3;
                break;

            case "Equipment_Mouth":
                i = 4;
                break;

            case "Equipment_Glove":
                i = 5;
                break;

            case "Equipment_Boot":
                i = 6;
                break;

            case "Equipment_Body":
                i = 7;
                break;

            case "Equipment_OuterBody":
                i = 8;
                break;

            case "Equipment_Back":
                i = 9;
                break;

            case "Equipment_Charm1":
                i = 10;
                break;

            case "Equipment_Charm2":
                i = 11;
                break;

            case "Equipment_Charm3":
                i = 12;
                break;

            case "Equipment_Weapon":
                i = 13;
                break;

            case "Equipment_Trinket1":
                i = 14;
                break;

            case "Equipment_Trinket2":
                i = 15;
                break;

            case "Equipment_Trinket3":
                i = 16;
                break;

            default:
                i = 10;
                Debug.Log("Equipment slot '" + Inventory_UI_Manager.SelectedEquipmentSlot + "' not recognized");
                break;
        }

        EquipmentSlots[i] = equipment;
        Debug.Log(EquipmentSlots[i].itemID);
        if (equipment.itemIcon != null)
        {
            EquipmentSlotVisuals[i].sprite = equipment.itemIcon;
        }
        else
        {
            EquipmentSlotVisuals[i].sprite = Resources.Load<Sprite>("Images/UI/Blank");
        }

        UpdateEquipmentEffects();
    }


    public void UnequipItem()
    {

        int i = 0;
        switch (Inventory_UI_Manager.SelectedEquipmentSlot)
        {
            case "Equipment_Headgear":
                i = 0;
                break;

            case "Equipment_Hat":
                i = 1;
                break;

            case "Equipment_Face":
                i = 2;
                break;

            case "Equipment_Eye":
                i = 3;
                break;

            case "Equipment_Mouth":
                i = 4;
                break;

            case "Equipment_Glove":
                i = 5;
                break;

            case "Equipment_Boot":
                i = 6;
                break;

            case "Equipment_Body":
                i = 7;
                break;

            case "Equipment_OuterBody":
                i = 8;
                break;

            case "Equipment_Back":
                i = 9;
                break;

            case "Equipment_Charm1":
                i = 10;
                break;

            case "Equipment_Charm2":
                i = 11;
                break;

            case "Equipment_Charm3":
                i = 12;
                break;

            case "Equipment_Weapon":
                i = 13;
                break;

            case "Equipment_Trinket1":
                i = 14;
                break;

            case "Equipment_Trinket2":
                i = 15;
                break;

            case "Equipment_Trinket3":
                i = 16;
                break;

            default:
                i = 10;
                break;
        }

        EquipmentSlots[i] = null;
        EquipmentSlotVisuals[i].sprite = Resources.Load<Sprite>("Images/UI/Blank");

        UpdateEquipmentEffects();
    }

    public void UpdateEquipmentEffects()
    {
        tenacityFromEquipment = 0;
        cognitionFromEquipment = 0;
        influenceFromEquipment = 0;
        luckFromEquipment = 0;

        armor = 0;


        foreach (Equipment equipment in EquipmentSlots)
        {
            if (equipment != null)
            {

                tenacityFromEquipment += equipment.tenacityModifier;
                cognitionFromEquipment += equipment.cognitionModifier;
                influenceFromEquipment += equipment.influenceModifier;
                luckFromEquipment += equipment.luckModifier;

                armor += equipment.armor;
            }
        }

        tenacityStat = naturalTenacity + tenacityFromEquipment;
        cognitionStat = naturalCognition + cognitionFromEquipment;
        influenceStat = naturalInfluence + influenceFromEquipment;
        luckStat = naturalLuck + luckFromEquipment;
    }

    public void SetUI()
    {
        FleshHPUI.sizeDelta = new Vector2(fleshHP * 2, 20);
        TempDamageUI.sizeDelta = new Vector2(damageTemporary * 2, 20);
        PermDamageUI.sizeDelta = new Vector2(damagePermanent * 2, 20);

        tenText.text = naturalTenacity.ToString();
        if (tenacityFromEquipment > 0)
        {
            tenText.text = tenText.text + " <color=green>+ " + tenacityFromEquipment.ToString() + "</color>";
        }
        else if (tenacityFromEquipment < 0)
        {
            tenText.text = tenText.text + " <color=red>" + tenacityFromEquipment.ToString() + "</color>";
        }

        cogText.text = naturalCognition.ToString();
        if (cognitionFromEquipment > 0)
        {
            cogText.text = cogText.text + " <color=green>+ " + cognitionFromEquipment.ToString() + "</color>";
        }
        else if (cognitionFromEquipment < 0)
        {
            cogText.text = cogText.text + " <color=red>" + cognitionFromEquipment.ToString() + "</color>";
        }

        infText.text = naturalInfluence.ToString();
        if (influenceFromEquipment > 0)
        {
            infText.text = infText.text + " <color=green>+ " + influenceFromEquipment.ToString() + "</color>";
        }
        else if (influenceFromEquipment < 0)
        {
            infText.text = infText.text + " <color=red>" + influenceFromEquipment.ToString() + "</color>";
        }
    }

    public void TakeDamage(string damageType, int damageAmount)
    {
        if (damageType == "TEMPORARY" || damageType == "Temporary" || damageType == "temporary")
        {
            if (armor >= damageAmount)
            {
                damageAmount = 0;
                SetUI();
                return;
            }
            else
            {
                damageAmount -= armor;
            }


            damageTemporary += damageAmount;
            fleshHP -= damageAmount;
            if (fleshHP <= 0)
            {
                damagePermanent -= fleshHP;
                damageTemporary = maxHP - damagePermanent;
                fleshHP = 0;
            }
        }
        else if (damageType == "PERMANENT" || damageType == "Permanent" || damageType == "permanent")
        {
            if (armor >= damageAmount)
            {
                TakeDamage("Temporary", damageAmount);
                return;
            }
            else
            {
                TakeDamage("Temporary", armor);
                damageAmount -= armor;
            }

            if (damageAmount > fleshHP) // e.g 30 dmg 20 fleshHP
            {
                damagePermanent += fleshHP;
                float overDamage = damageAmount - fleshHP;
                damagePermanent += (overDamage * 2);
                fleshHP = 0;
            }
            else
            {
                damagePermanent += damageAmount;
                fleshHP -= damageAmount;
            }

            if (fleshHP < 0)
            {
                fleshHP = 0;
            }

            damageTemporary = maxHP - damagePermanent - fleshHP;
        }
        else
        {
            Debug.Log("Damage type '" + damageType + "' not recognised.");
        }
        if (fleshHP < 0)
        {
            fleshHP = 0;
        }
        if (damageTemporary < 0)
        {
            damageTemporary = 0;
        }
        if (damagePermanent >= maxHP)
        {
            damagePermanent = maxHP;
            Debug.Log("Player is dead.");
        }

        SetUI();
        healthiness = (maxHP - (damageTemporary / 2) - damagePermanent) / maxHP;
    }


}
