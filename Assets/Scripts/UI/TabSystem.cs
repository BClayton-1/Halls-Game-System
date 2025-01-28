using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    public class TabSystem : MonoBehaviour
    {
        void Awake()
        {
            tabsArray = new UITab[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                tabsArray[i] = gameObject.transform.GetChild(i).GetComponent<UITab>();
            }
        }

        UITab[] tabsArray;

        public void UnselectOtherTabs(int index)
        {
            for (int i = 0; i < tabsArray.Length; i++)
            {
                if (i == index)
                {
                    continue;
                }
                tabsArray[i].UnselectTab();
            }
        }

        private void OnEnable()
        {
            tabsArray[0].SelectTab();
        }
    }
}