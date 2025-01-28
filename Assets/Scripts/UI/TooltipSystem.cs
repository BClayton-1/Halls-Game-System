using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame
{
    public class TooltipSystem : MonoBehaviour
    {
        public static TooltipSystem current;
        public Tooltip tooltip;

        public void Awake()
        {
            current = this;
        }

        public static void Show(string content, string header = "")
        {
            current.tooltip.SetText(content, header);
            current.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            current.tooltip.gameObject.SetActive(false);
        }

        public static void SetPos(Vector2 pos, float offset)
        {
            pos.x = pos.x + offset;
            current.tooltip.gameObject.transform.position = pos;
        }
    }
}