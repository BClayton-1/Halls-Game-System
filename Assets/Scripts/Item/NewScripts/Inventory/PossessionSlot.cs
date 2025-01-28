using System;
using UnityEngine;

namespace MeatGame.Possession
{
    internal class PossessionSlot
    {
        public Possession possession { get; private set; }
        public int quantity { get; private set; }
        public int minQuantity { get; private set; }
        public int maxQuantity { get; private set; }

        public PossessionSlot(Possession _possession, int _quantity = 0)
        {
            switch (_possession.type)
            {
                case PossessionType n when ((int)n < 20):
                    minQuantity = 0;
                    maxQuantity = 999;
                    break;
                case PossessionType n when ((int)n >= 20 && (int)n < 30):
                    minQuantity = -100;
                    maxQuantity = 100;
                    break;
                default:
                    minQuantity = 0;
                    maxQuantity = 1;
                    break;
            }
            possession = _possession;
            quantity = _quantity;
        }

        public void AddQuantity(int quantityInput)
        {
            quantity = Mathf.Min((quantity + quantityInput), maxQuantity);
        }

        public void RemoveQuantity(int quantityInput)
        {
            quantity = Mathf.Max((quantity - quantityInput), minQuantity);
        }

        public void SetQuantity(int quantityInput)
        {
            quantity = Mathf.Clamp(quantityInput, minQuantity, maxQuantity);
        }

        public bool RemoveSlotCheck()
        {
            if ((int)possession.type == 20 || (int)possession.type == 21) // Possessions of type Relation and Reputation continue to exist in inventory regardless of quantity
            {
                return false;
            }
            if (quantity == minQuantity)
            {
                return true;
            }
            return false;
        }

    }
}
