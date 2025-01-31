using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.ThreeD;
using MeatGame.Possession.UI;

namespace MeatGame
{
    public class PlayerMenu : MonoBehaviour
    {
        public static PlayerMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            playerMenuObject = gameObject.transform.GetChild(1).gameObject;
        }

        public void AssignMenuFreeze(MenuFreeze _menuFreeze)
        {
            menuFreeze = _menuFreeze;

        }
        private MenuFreeze menuFreeze;

        public void TryCheckMenuFreeze()
        {
            if (menuFreeze != null)
            {
                menuFreeze.CheckFreezeMenu();
            }
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyBinds.Instance.keyTogglePlayerMenu))
            {
                if (menuIsOpen)
                {
                    ClosePlayerMenu();
                }
                else if (playerMenuUseAllowed)
                {
                    OpenPlayerMenu();
                }
                TryCheckMenuFreeze();
            }
        }

        public bool menuIsOpen { get; private set; } = false;

        public void OpenPlayerMenu()
        {
            playerMenuObject.SetActive(true);
            menuIsOpen = true;
        }

        public void ClosePlayerMenu()
        {
            playerMenuObject.SetActive(false);
            TooltipSystem.Instance.Hide();
            UsableMenuSystem.Instance.CloseUseMenu();
            menuIsOpen = false;
        }

        private GameObject playerMenuBGObject;

        public GameObject playerMenuObject { get; private set; }

        private bool playerMenuUseAllowed = true;

        public void EnableMenuUse()
        {
            playerMenuUseAllowed = true;
        }

        public void DisableMenuUse()
        {
            playerMenuUseAllowed = false;
        }
    }
}