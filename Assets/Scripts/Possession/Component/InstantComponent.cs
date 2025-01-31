using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

namespace MeatGame.Possession.Component
{
    internal sealed class InstantComponent : IUsableComponent
    {
        public string UseText { get; set; }

        public InstantComponent(XElement _XElement)
        {
            UseText = _XElement.Attribute("usetext").Value;

            foreach (XElement subElement in _XElement.Elements())
            {
                string subElementName = subElement.Name.ToString().ToLowerInvariant();
                if (subElementName != null)
                {
                    switch(subElementName)
                    {
                        case "addpossession":
                            string _identifier = subElement.Attribute("identifier").Value.ToString();
                            int _quantity;
                            int.TryParse(subElement.Attribute("quantity").Value, out _quantity);
                            AddPossession addPossession = new AddPossession(_identifier, _quantity, true);
                            instantsList.Add(addPossession);
                            break;
                        case "removepossession":
                            _identifier = subElement.Attribute("identifier").Value.ToString();
                            int.TryParse(subElement.Attribute("quantity").Value, out _quantity);
                            addPossession = new AddPossession(_identifier, _quantity, false);
                            instantsList.Add(addPossession);
                            break;
                    }
                }
            }
        }

        private List<AddPossession> instantsList = new List<AddPossession>();

        public void Use()
        {
            foreach (AddPossession addPossession in instantsList)
            {
                if (addPossession.isPositive)
                {
                    InventoryManager.Instance.AddPossession(addPossession.identifier, addPossession.quantity);
                    return;
                }
                InventoryManager.Instance.RemovePossession(addPossession.identifier, addPossession.quantity);
            }
        }


    }

    internal struct AddPossession
    {
        public AddPossession(string _identifier, int _quantity, bool _isPositive)
        {
            identifier = _identifier;
            quantity = _quantity;
            isPositive = _isPositive;
        }

        public string identifier { get; private set; }
        public int quantity { get; private set; }
        public bool isPositive { get; private set; }
    }
}