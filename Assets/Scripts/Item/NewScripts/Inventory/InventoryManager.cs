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
            if (!playerInventory.ContainsKey(identifier))
            {
                Possession possession = PossessionManager.Instance.GetPossession(identifier);
                PossessionSlot possessionSlotToAdd = new PossessionSlot(possession);
                possessionSlotToAdd.AddQuantity(quantity);
                playerInventory.Add(identifier, possessionSlotToAdd);
                InventoryUI.Instance.AddInventorySlotUI(possessionSlotToAdd); // MeatGame.Inventory.UI
                return;
            }
            playerInventory[identifier].AddQuantity(quantity);
            InventoryUI.Instance.UpdateExistingSlotUI(playerInventory[identifier]); // MeatGame.Inventory.UI
        }

        /*private void SortInventory()
        {
            if (playerInventory.Count() < 2)
            {
                return;
            }
            //from kvp in playerInventory orderby kvp.Value.possession.sortingID ascending select kvp;
            playerInventory = playerInventory.OrderBy(key => key.Value.possession.sortingID);
            foreach (KeyValuePair<string, PossessionSlot> kvp in playerInventory)
            {
                Debug.Log(kvp.Key);
            }
        }*/

        public void RemovePossession(string identifier, int quantity = 1)
        {
            if (quantity < 1)
            {
                Debug.Log("RemovePossession() cannot take a value below 1. Ignoring.");
                return;
            }
            if (!playerInventory.ContainsKey(identifier))
            {
                Debug.Log("No " + identifier + " found in inventory when trying to RemovePossession().");
                return;
            }
            playerInventory[identifier].RemoveQuantity(quantity);
            if (playerInventory[identifier].RemoveSlotCheck())
            {
                playerInventory.Remove(identifier);
                InventoryUI.Instance.RemoveInventorySlotUI(identifier); // MeatGame.Inventory.UI
                return;
            }
            InventoryUI.Instance.UpdateExistingSlotUI(playerInventory[identifier]); // MeatGame.Inventory.UI
        }

        public int GetQuantity(string identifier)
        {
            if (playerInventory.ContainsKey(identifier))
            {
                return playerInventory[identifier].quantity;
            }
            return 0;
        }
    }
}
