using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.Dialogue;

namespace MeatGame
{
    public class SaveData : MonoBehaviour
    {
        public static SaveData Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        /* Script Dependencies
        DialogueManager
        TimeManager
        */

        // Viewed dialogues
        public List<string> ViewedDialogue = new List<string>();

        // Day and time
        private int day;
        private int hour;
        private int minute;

        // Player variables
        private int tenacity;
        private int cognition;
        private int influence;
        private int luck;

        private float maxHP;
        private float fleshHP;
        private float damageTemporary;
        private float damagePermanent;

        private int composure;
        private int sanity;

        // Position
        private bool inThreeDEnvironment;
        private string twoDRoom;
        private string threeDScene;
        private Vector3 threeDPosition;



        public void SaveGame()
        {
            day = TimeManager.Instance.day;
            hour = TimeManager.Instance.hour;
            minute = TimeManager.Instance.minute;

        }

    }
}