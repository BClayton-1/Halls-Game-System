using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MeatGame.Possession
{
    internal class PossessionManager : MonoBehaviour
    {
        public static PossessionManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            BuildPossessionDict();
        }

        private void BuildPossessionDict()
        {
            XElement possessions_XML = XElement.Load("Assets/Resources/Data/possessions.xml");
            IEnumerable<XElement> possessionXElements = possessions_XML.Elements();
            int index = 0; // Used to keep track of the order the possessions are added in
            foreach (var possession in possessionXElements)
            {
                index++;
                string _identifier = possession.Attribute("identifier").Value;
                string _name = possession.Attribute("name").Value;
                PossessionType _type;
                PossessionType.TryParse(possession.Attribute("type").Value, out _type);
                string _description = possession.Attribute("description").Value;
                Sprite _inventoryIcon = null;
                if (possession.Attribute("inventoryicon") != null)
                {
                    string inventoryIconPath = possession.Attribute("inventoryicon").Value;
                    _inventoryIcon = Resources.Load<Sprite>("Images/Possessions/InventoryIcon/" + inventoryIconPath);
                }
                if (_type < PossessionType.Trinket)
                {
                    Possession possessionToAdd = new Possession(_identifier, _name, _type, index, _description, _inventoryIcon);
                    possessionDict.Add(_identifier, possessionToAdd);
                }
                Debug.Log(_identifier + " loaded");
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