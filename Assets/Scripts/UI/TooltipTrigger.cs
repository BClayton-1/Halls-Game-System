using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MeatGame
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string content;
        public string header;

        [SerializeField] private RectTransform rectTransformA;

        void Start()
        {
            rectTransformA = gameObject.GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            float offset = (GetWorldRect(rectTransformA).width / 2) + 2;
            TooltipSystem.SetPos(transform.position, offset);
            TooltipSystem.Show(content, header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }

        private Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            // Get the bottom left corner.
            Vector3 position = corners[0];

            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);

            return new Rect(position, size);
        }
    }
}