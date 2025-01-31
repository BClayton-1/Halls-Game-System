using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.Possession.Component;

namespace MeatGame.Possession.UI
{
    internal class UsableMenuSystem : MonoBehaviour
    {
        public static UsableMenuSystem Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private GameObject rightClickMenuObject;
        [SerializeField] private GameObject closeMenuObject;
        [SerializeField] private GameObject useMenuButtonPrefab;

        public void OpenUseMenu(List<IUsableComponent> _currentUsableComponents, string _possessionIdentifier, RectTransform slotTransform)
        {
            Vector3[] corners = new Vector3[4];
            slotTransform.GetWorldCorners(corners);
            Vector3 targetPosition = corners[2];
            targetPosition.x += 5;
            rightClickMenuObject.transform.position = targetPosition;
            TooltipSystem.Instance.Hide();
            CloseUseMenu();
            closeMenuObject.SetActive(true);
            activePossessionIdentifier = _possessionIdentifier;
            foreach (IUsableComponent usableComponent in _currentUsableComponents)
            {
                GameObject useMenuButton = Instantiate(useMenuButtonPrefab, rightClickMenuObject.transform);
                useMenuButton.GetComponent<UsableButton>().Init(usableComponent);
            }
            rightClickMenuObject.SetActive(true);
        }

        public string activePossessionIdentifier { get; private set; }

        public void CloseUseMenu()
        {
            closeMenuObject.SetActive(false);
            for (int i = rightClickMenuObject.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(rightClickMenuObject.transform.GetChild(i).gameObject);
            }
            rightClickMenuObject.SetActive(false);
        }
    }
}