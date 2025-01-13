using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    internal class Possession
    {
        public Possession(string _identifier, string _name, PossessionType _type, int sortingID, string _description = "", Sprite _inventoryIcon = null)
        {
            switch (_type)
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
            identifier = _identifier;
            name = _name;
            type = _type;
            description = _description;
            inventoryIcon = _inventoryIcon;
        }

        public string identifier { get; private set; }
        public string name { get; private set; }
        public PossessionType type { get; private set; }
        public string description { get; private set; }
        public Sprite inventoryIcon { get; private set; }

        public int minQuantity { get; private set; }
        public int maxQuantity { get; private set; }

        public int sortingID { get; private set; }
    }
}
