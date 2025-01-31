using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MeatGame.Possession.UI;

namespace MeatGame.Possession
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private Dictionary<string, PossessionSlot> playerInventory = new Dictionary<string, PossessionSlot>();

        public void AddPossession(string identifier, int quantity = 1)
        {
            if (quantity < 1)
            {
                Debug.Log("AddPossession() cannot take a value below 1. Ignoring.");
                return;
            }
            Debug.Log(identifier);
            if (!playerInventory.ContainsKey(identifier))
            {
                Possession possession = PossessionManager.Instance.GetPossession(identifier);
                PossessionSlot possessionSlotToAdd = new PossessionSlot(possession);
                possessionSlotToAdd.AddQuantity(quantity);
                playerInventory.Add(identifier, possessionSlotToAdd);
                InventoryUI.Instance.AddInventorySlotUI(possessionSlotToAdd); // UI
                return;
            }
            playerInventory[identifier].AddQuantity(quantity);
            InventoryUI.Instance.UpdateExistingSlotUI(playerInventory[identifier]); // UI
        }

        public void RemovePossession(string _identifier, int _quantity = 1)
        {
            if (_quantity < 1)
            {
                Debug.Log("RemovePossession() cannot take a value below 1. Ignoring.");
                return;
            }
            if (!playerInventory.ContainsKey(_identifier.ToString()))
            {
                Debug.Log("No " + _identifier + " found in inventory when trying to RemovePossession().");
                return;
            }
            playerInventory[_identifier].RemoveQuantity(_quantity);
            if (playerInventory[_identifier].RemoveSlotCheck())
            {
                playerInventory.Remove(_identifier);
                InventoryUI.Instance.RemoveInventorySlotUI(_identifier); // UI
                return;
            }
            InventoryUI.Instance.UpdateExistingSlotUI(playerInventory[_identifier]); // UI
        }

        public int GetQuantity(string _identifier)
        {
            if (playerInventory.ContainsKey(_identifier))
            {
                return playerInventory[_identifier].quantity;
            }
            return 0;
        }

        public bool ContainsPossession(string _identifier)
        {
            if (playerInventory.ContainsKey(_identifier))
            {
                return true;
            }
            return false;
        }

        /*public void PrintCurrentInventory()
        {
            foreach (KeyValuePair<string, PossessionSlot> kvp in playerInventory)
            {
                Debug.Log(kvp.Key + ": " + kvp.Value.quantity);
            }
        }*/
    }
}
