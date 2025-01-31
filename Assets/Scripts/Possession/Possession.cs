using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using MeatGame.Possession.Component;

namespace MeatGame.Possession
{
    internal class Possession
    {
        /*public Possession(string _identifier, string _name, PossessionType _type, int sortingID, string _description = "", Sprite _inventoryIcon = null)
        {
            identifier = _identifier;
            name = _name;
            type = _type;
            description = _description;
            inventoryIcon = _inventoryIcon;
        }*/

        public Possession(XElement possessionXElement, int _sortingID)
        {
            sortingID = _sortingID;
            identifier = possessionXElement.Attribute("identifier").Value;
            name = possessionXElement.Attribute("name").Value;
            PossessionType _type;
            PossessionType.TryParse(possessionXElement.Attribute("type").Value, out _type);
            type = _type;
            description = possessionXElement.Attribute("description").Value;
            if (possessionXElement.Attribute("inventoryicon") != null)
            {
                string inventoryIconPath = possessionXElement.Attribute("inventoryicon").Value;
                inventoryIcon = Resources.Load<Sprite>("Images/Possessions/InventoryIcon/" + inventoryIconPath);
            }

            if (possessionXElement.Element("Usable") != null)
            {
                isUsable = true;
                foreach (XElement usableComponentXElement in possessionXElement.Element("Usable").Elements())
                {
                    string elementName = usableComponentXElement.Name.ToString().ToLowerInvariant();
                    if (elementName != null)
                    {
                        switch(elementName)
                        {
                            case "instant":
                                InstantComponent instantComponent = new InstantComponent(usableComponentXElement);
                                usableComponents.Add(instantComponent);
                                break;
                            case "equipment":
                                //EquipmentComponent equipmentComponent = new EquipmentComponent(usableComponentXElement);
                                //usableComponents.Add(equipmentComponent);
                                break;
                        }
                    }
                }
            }
            //Debug.Log(identifier + " loaded");
        }

        public List<IUsableComponent> usableComponents { get; private set; } = new List<IUsableComponent>();
        public bool isUsable { get; private set; } = false;

        public string identifier { get; private set; }
        public string name { get; private set; }
        public PossessionType type { get; private set; }
        public string description { get; private set; }
        public Sprite inventoryIcon { get; private set; }

        public int sortingID { get; private set; }
    }
}
