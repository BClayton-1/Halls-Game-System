using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MeatGame.inventory
{
    internal class InventoryManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

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
                Possession possession = PossessionManager.instance.GetPossession(identifier);
                PossessionSlot possessionSlotToAdd = new PossessionSlot(possession);
                possessionSlotToAdd.AddQuantity(quantity);
                playerInventory.Add(identifier, possessionSlotToAdd);
                SortInventory();
                return;
            }
            playerInventory[identifier].AddQuantity(quantity);
        }

        private void SortInventory()
        {
            if (playerInventory.Count() < 2)
            {
                return;
            }
            var result = from kvp in playerInventory orderby kvp.Value.possession.sortingID ascending select kvp;
            playerInventory = (Dictionary<string, PossessionSlot>)result;
        }

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
            }
        }
    }
}
