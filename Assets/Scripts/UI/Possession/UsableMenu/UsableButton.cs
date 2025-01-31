using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MeatGame.Possession.Component;

namespace MeatGame.Possession.UI
{
    internal class UsableButton : MonoBehaviour
    {
        public void Init(IUsableComponent _usableComponent)
        {
            useText.text = _usableComponent.UseText;
            usableComponent = _usableComponent;
        }

        [SerializeField] private TextMeshProUGUI useText;
        private IUsableComponent usableComponent;

        public void PressButton()
        {
            usableComponent.Use();
            if (!InventoryManager.Instance.ContainsPossession(UsableMenuSystem.Instance.activePossessionIdentifier)) // If executing Use() results in the possession being completely removed from the player inventory
            {
                UsableMenuSystem.Instance.CloseUseMenu();
            }
        }
    }
}