using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEquipmentSlot : MonoBehaviour
{
    public PlayerManager Player_Manager;

    public Equipment equipment;

    public TooltipTrigger Tooltip_Trigger;

    [SerializeField] private TextMeshProUGUI itemNameText;


    public void SetEquipment()
    {
        Player_Manager.EquipItem(equipment);
    }

    void Start()
    {
        itemNameText.text = Tooltip_Trigger.header;
    }

}
