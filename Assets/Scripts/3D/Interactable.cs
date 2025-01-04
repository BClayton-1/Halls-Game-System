using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public string interactText = "Interact";



    public virtual void TriggerInteract()
    {
        Debug.Log("Interaction triggered");
    }
}
