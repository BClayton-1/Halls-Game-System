using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    public class TooltipSystem : MonoBehaviour
    {
        public static TooltipSystem Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private Tooltip tooltip;

        public void Show(string content, string header = "")
        {
            tooltip.SetText(content, header);
            tooltip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }

        public void SetPos(Vector2 pos, float offset)
        {
            pos.x = pos.x + offset;
            tooltip.gameObject.transform.position = pos;
        }
    }
}