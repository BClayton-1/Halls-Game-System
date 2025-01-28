using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MeatGame.ThreeD
{
    public class PlayerInteract : MonoBehaviour
    {
        /* Script Dependencies
        KeyBinds
        */

        public TextMeshProUGUI interactTextUI;

        // Start is called before the first frame update
        void Start()
        {
            interactTextUI = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactTextUI.text = "<color=orange><uppercase>[" + KeyBinds.Instance.keyInteract.ToString() + "]</uppercase></color> " + interactable.interactText;
                    if (Input.GetKeyDown(KeyBinds.Instance.keyInteract))
                    {
                        interactable.TriggerInteract();
                        interactTextUI.text = string.Empty;
                    }
                }
                else
                {
                    interactTextUI.text = string.Empty;
                }
            }
            else
            {
                interactTextUI.text = string.Empty;
            }
        }
    }
}