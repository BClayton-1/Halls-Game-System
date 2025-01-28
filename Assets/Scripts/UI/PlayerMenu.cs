using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.ThreeD;

namespace MeatGame
{
    public class PlayerMenu : MonoBehaviour
    {
        public static PlayerMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            playerMenuObject = this.gameObject.transform.GetChild(1).gameObject;
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
            if(Input.GetKeyDown(KeyBinds.Instance.keyTogglePlayerMenu) && playerMenuUseAllowed)
            {
                playerMenuObject.SetActive(!playerMenuObject.activeSelf); // Toggle for now
                TryCheckMenuFreeze();
            }
        }

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