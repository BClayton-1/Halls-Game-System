using System.Collections;
using System.Collections.Generic;

namespace MeatGame.Possession
{
    internal enum PossessionType
    {
        // Item: 0-19
        Utility = 0,
        Flesh = 1,
        Junk = 2,
        Oddity = 3,
        Jar = 4,
        TrinketItem = 18,
        WearableItem = 19,

        // Asset: 20-39
        Memory = 20,
        Lore = 21,

        // Learned: 40-59

        // Rep: 60-64
        Reputation = 60,
        Relation = 61,

        // Trinket: 65
        Trinket = 65,

        // Wearable: 70-89
        Hat = 72,
        Eye = 73,
        Mouth = 74,
        Face = 75,
        Glove = 76,
        OuterBody= 77,
        Body = 78,
        Boot = 79,
        //Character = 80,
        Back = 81,
        Weapon = 82
    }

    internal enum PossessionGroup
    {
        Item = 0,
        Asset = 1,
        Learned = 2,
        Reputation = 3,
        Trinket = 4,
        Wearable = 5
    }

    internal class PossessionTypeMethods
    {
        public PossessionGroup GetPossessionGroup(PossessionType _possessionType)
        {
            switch (_possessionType)
            {
                case PossessionType n when ((int)n < 20):
                    return PossessionGroup.Item;
                case PossessionType n when ((int)n >= 20 && (int)n < 40):
                    return PossessionGroup.Asset;
                case PossessionType n when ((int)n >= 40 && (int)n < 60):
                    return PossessionGroup.Learned;
                case PossessionType n when ((int)n >= 60 && (int)n < 65):
                    return PossessionGroup.Reputation;
                case PossessionType n when ((int)n == 65):
                    return PossessionGroup.Trinket;
                case PossessionType n when ((int)n >= 70 && (int)n < 90):
                    return PossessionGroup.Reputation;
                default:
                    return PossessionGroup.Item;
            }
        }

        public string GetName(PossessionType _possessionType)
        {
            if (PossessionTypeSpecialNames.ContainsKey(_possessionType))
            {
                return PossessionTypeSpecialNames[_possessionType];
            }
            return _possessionType.ToString();
        }

        Dictionary<PossessionType, string> PossessionTypeSpecialNames = new Dictionary<PossessionType, string> // Names whose values are different than in the enum
        {
            {PossessionType.TrinketItem, "Trinket"},
            {PossessionType.WearableItem, "Equipment"},
            {PossessionType.OuterBody, "Outer Body"}
        };
    }
}
