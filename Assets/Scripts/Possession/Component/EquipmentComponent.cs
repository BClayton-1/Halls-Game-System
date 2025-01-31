using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

namespace MeatGame.Possession.Component
{
    public class EquipmentComponent : IUsableComponent
    {
        public string UseText { get; set; }

        public EquipmentComponent(XElement _XElement)
        {
            UseText = "Equip";
        }

        public void Use()
        {
            // Equip possession to correct slot after unequipping anything in said slot
        }
    }
}