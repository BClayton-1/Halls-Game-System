using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MeatGame
{
    internal class PossessionManager : MonoBehaviour
    {
        public static PossessionManager instance;

        // Start is called before the first frame update
        void Start()
        {
            BuildPossessionDict();
        }

        private void BuildPossessionDict()
        {
            XElement possessions_XML = XElement.Load("possessions.xml");
            IEnumerable<XElement> possessionXElements = possessions_XML.Elements();
            
            for (int i = 0; i < possessionXElements.Count(); i++)
            {
                var possession = possessionXElements.ElementAt(i);
                string _identifier = possession.Element("identifier").Value;
                string _name = possession.Element("name").Value;
                PossessionType _type;
                PossessionType.TryParse(possession.Element("type").Value, out _type);
                string _description = possession.Element("description").Value;
                string inventoryIconPath = possession.Element("inventoryicon").Value;
                Sprite _inventoryIcon = Resources.Load<Sprite>("Images/Possessions/InventoryIcon" + inventoryIconPath);
                if (_type < PossessionType.Trinket)
                {
                    Possession possessionToAdd = new Possession(_identifier, _name, _type, i, _description, _inventoryIcon);
                    possessionDict.Add(_identifier, possessionToAdd);
                }

            }
        }

        private Dictionary<string, Possession> possessionDict = new Dictionary<string, Possession>();

        public Possession GetPossession(string identifier)
        {
            if (possessionDict.ContainsKey(identifier))
            {
                return possessionDict[identifier];
            }
            Debug.Log("No Possession with identifier of " + identifier + " found.");
            return null;
        }

    }
}