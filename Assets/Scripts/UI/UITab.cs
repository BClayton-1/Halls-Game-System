using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeatGame
{
    internal class UITab : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            tabSystem = this.gameObject.transform.parent.GetComponent<TabSystem>();
        }

        public void SelectTab()
        {
            Image image = gameObject.GetComponent<Image>();
            image.sprite = selectedSprite;
            image.color = selectedColor;
            connectedPage.SetActive(true);
            tabSystem.UnselectOtherTabs(gameObject.transform.GetSiblingIndex());
        }

        public void UnselectTab()
        {
            Image image = gameObject.GetComponent<Image>();
            image.sprite = unselectedSprite;
            image.color = unselectedColor;
            connectedPage.SetActive(false);
        }

        private TabSystem tabSystem;
        [SerializeField] private GameObject connectedPage;

        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Color selectedColor;

        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Color unselectedColor;
    }
}