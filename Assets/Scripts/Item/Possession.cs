using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    internal class Possession
    {
        public Possession(string _identifier, string _name, PossessionType _type, string _description = "", Sprite _inventoryIcon = null, int _minQuantity = 0, int _maxQuantity = 999)
        {
            if (_type >= PossessionType.Trinket)
            {
                _minQuantity = 0;
                _maxQuantity = 1;
            }
            identifier = _identifier;
            name = _name;
            type = _type;
            description = _description;
            inventoryIcon = _inventoryIcon;
            minQuantity = _minQuantity;
            maxQuantity = _maxQuantity;
        }

        public string identifier;
        public string name;
        public PossessionType type;
        public string description;
        public Sprite inventoryIcon;
        public int minQuantity;
        public int maxQuantity;

        public int quantity = 0;

        public enum PossessionType
        {
            Item,
            Memory,
            Relation,
            Favor,
            // Equipment
            Trinket,
            Weapon,
            Hat,
            Face,
            Eye,
            Mouth,
            Glove,
            Boot,
            Body,
            OuterBody,
            Back
        }
    }
}
