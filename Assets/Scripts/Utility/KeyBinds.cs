using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    public class KeyBinds : MonoBehaviour
    {
        public static KeyBinds Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public KeyCode keyMoveForward { get; private set; } = KeyCode.W;
        public KeyCode keyStrafeLeft { get; private set; } = KeyCode.A;
        public KeyCode keyMoveBackward { get; private set; } = KeyCode.S;
        public KeyCode keyStrafeRight { get; private set; } = KeyCode.D;

        public KeyCode keyInteract { get; private set; } = KeyCode.E;
        public KeyCode keyTogglePlayerMenu { get; private set; } = KeyCode.I;
    }
}