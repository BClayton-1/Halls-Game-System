using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MeatGame.Possession.Component;

namespace MeatGame.Possession.UI
{
    internal class UsableMenuTrigger : MonoBehaviour, IPointerClickHandler
    {
        public void SetUsableComponents(List<IUsableComponent> _usableComponents)
        {
            usableComponents = _usableComponents;
        }
        private List<IUsableComponent> usableComponents;
        public string possessionIdentifier { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                UsableMenuSystem.Instance.OpenUseMenu(usableComponents, possessionIdentifier, GetComponent<RectTransform>());
            }
        }
    }
}