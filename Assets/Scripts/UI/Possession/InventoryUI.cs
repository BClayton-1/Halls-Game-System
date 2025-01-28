using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame.Possession.UI
{
    internal class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        void Start() // Create grids and labels for each possession type. The type of possesion determines where the grid will be instantiated
        {
            for (int i = 0; i < 20; i++) // 85 is aprox length of PossessionType, with some room for new equipment. May need to be increased if more equipment types are added
            {
                PossessionType possessionType = (PossessionType)i;
                if (!Enum.IsDefined(typeof(PossessionType), i))
                {
                    continue;
                }

                // Possession group determines where grid is instantiated
                PossessionGroup possessionGroup = possessionTypeMethods.GetPossessionGroup(possessionType);
                Transform instantiationTransform;
                switch (possessionGroup)
                {
                    case PossessionGroup.Item:
                        instantiationTransform = itemInventoryParent.transform;
                        break;
                    case PossessionGroup.Asset:
                        instantiationTransform = itemInventoryParent.transform; //Needs to be changed
                        break;
                    case PossessionGroup.Learned:
                        instantiationTransform = itemInventoryParent.transform; //Needs to be changed
                        break;
                    case PossessionGroup.Reputation:
                        instantiationTransform = itemInventoryParent.transform; //Needs to be changed
                        break;
                    default:
                        instantiationTransform = itemInventoryParent.transform;
                        break;
                }

                GameObject groupUIObject = Instantiate(InventoryGroupPrefab, instantiationTransform);
                string _name = possessionTypeMethods.GetName(possessionType);
                groupUIObject.name = (_name + " Group");
                InventoryGroupUI groupUIScript = groupUIObject.GetComponent<InventoryGroupUI>();
                groupUIScript.possessionTypeName = _name;

                groupUIDict.Add(possessionType, groupUIObject);
            }
        }

        private Dictionary<PossessionType, GameObject> groupUIDict = new Dictionary<PossessionType, GameObject>();

        PossessionTypeMethods possessionTypeMethods = new PossessionTypeMethods();

        
        [SerializeField] private GameObject InventoryGroupPrefab;

        [SerializeField] private GameObject itemInventoryParent;

        public void AddInventorySlotUI(PossessionSlot _possessionSlot)
        {
            Transform targetGridTransform = groupUIDict[_possessionSlot.possession.type].transform.GetChild(1);
            GameObject inventorySlotObject = Instantiate(InventorySlotPrefab, targetGridTransform);
            InventorySlotUI slotUIScript = inventorySlotObject.GetComponent<InventorySlotUI>();
            slotUIScript.Init(_possessionSlot);
            targetGridTransform.parent.gameObject.SetActive(true);
        }

        public void UpdateExistingSlotUI(PossessionSlot _possessionSlot)
        {
            GameObject inventorySlotObject = slotUIDict[_possessionSlot.possession.identifier];
            InventorySlotUI slotUIScript = inventorySlotObject.GetComponent<InventorySlotUI>();
            slotUIScript.Init(_possessionSlot);
        }

        public void RemoveInventorySlotUI(string identifier)
        {
            GameObject slotToRemove = slotUIDict[identifier];
            if (slotToRemove.transform.parent.childCount == 1)
            {
                slotToRemove.transform.parent.parent.gameObject.SetActive(false);
            }
            Destroy(slotToRemove);
        }

        private Dictionary<string, GameObject> slotUIDict = new Dictionary<string, GameObject>();

        [SerializeField] private GameObject InventorySlotPrefab;
    }
}
