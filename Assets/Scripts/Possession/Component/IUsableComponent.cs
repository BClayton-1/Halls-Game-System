using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

namespace MeatGame.Possession.Component
{
    internal interface IUsableComponent
    {
        public string UseText { get; }

        public void Use();
    }
}