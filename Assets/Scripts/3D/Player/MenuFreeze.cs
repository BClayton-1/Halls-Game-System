using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MeatGame.Dialogue;

namespace MeatGame.ThreeD
{
    public class MenuFreeze : MonoBehaviour
    {
        /* Script Dependencies
        DialogueManager
        */
        [SerializeField] private MouseLook Mouse_Look;
        [SerializeField] private PlayerMovement Player_Movement;

        void Awake()
        {
            PlayerMenu.Instance.AssignMenuFreeze(this);
            Transform UIManagerTransform = GameObject.Find("UIManager").transform;
            dialogueMenu = UIManagerTransform.GetChild(0).gameObject;
            playerMenu = UIManagerTransform.GetChild(1).gameObject;

            CheckFreezeMenu();
        }

        private GameObject dialogueMenu;
        private GameObject playerMenu;

        public void CheckFreezeMenu()
        {
            if (dialogueMenu.activeSelf == true || playerMenu.activeSelf == true)
            {
                Mouse_Look.enabled = false;
                Player_Movement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Mouse_Look.enabled = true;
                Player_Movement.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}