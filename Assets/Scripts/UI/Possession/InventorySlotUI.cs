using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MeatGame.Possession.UI
{
    internal class InventorySlotUI : MonoBehaviour
    {
        public void Init(PossessionSlot possessionSlot)
        {
            inventoryIcon.sprite = possessionSlot.possession.inventoryIcon;
            quantityText.text = possessionSlot.quantity.ToString();
            tooltipTrigger.header = possessionSlot.possession.name;
            tooltipTrigger.content = possessionSlot.possession.description;
            siblingIndex = possessionSlot.possession.sortingID;
        }
        
        public int siblingIndex;

        [SerializeField] private Image inventoryIcon;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TooltipTrigger tooltipTrigger;
    }
}