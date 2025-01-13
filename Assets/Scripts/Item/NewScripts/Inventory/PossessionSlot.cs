using System;
using UnityEngine;

namespace MeatGame.inventory
{
    internal class PossessionSlot
    {
        public Possession possession { get; private set; }
        public int quantity { get; private set; }

        public PossessionSlot(Possession _possession, int _quantity = 0)
        {
            possession = _possession;
            quantity = _quantity;
        }

        public void AddQuantity(int quantityInput)
        {
            quantity = Mathf.Min((quantity + quantityInput), possession.maxQuantity);
        }

        public void RemoveQuantity(int quantityInput)
        {
            quantity = Mathf.Max((quantity - quantityInput), possession.minQuantity);
        }

        public void SetQuantity(int quantityInput)
        {
            quantity = Mathf.Clamp(quantityInput, possession.minQuantity, possession.maxQuantity);
        }

        public bool RemoveSlotCheck()
        {
            if ((int)possession.type == 20 || (int)possession.type == 21) // Possessions of type Relation and Reputation continue to exist in inventory regardless of quantity
            {
                return false;
            }
            if (quantity == possession.minQuantity)
            {
                return true;
            }
            return false;
        }

    }
}
