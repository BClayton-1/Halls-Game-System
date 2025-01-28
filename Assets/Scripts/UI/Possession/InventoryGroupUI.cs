using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MeatGame.Possession.UI
{
    internal class InventoryGroupUI : MonoBehaviour
    {

        void Start()
        {
            GameObject headerObject = this.transform.GetChild(0).gameObject;
            TextMeshProUGUI headerText = headerObject.GetComponent<TextMeshProUGUI>();
            headerText.text = ("--" + possessionTypeName + "--");
            headerObject.name = (possessionTypeName + " Header");
        }

        // May or may not keep public
        public string possessionTypeName;
    }
}