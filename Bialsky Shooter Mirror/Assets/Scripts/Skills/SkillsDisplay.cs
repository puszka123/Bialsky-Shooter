using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.SkillSystem
{
    public class SkillsDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] Canvas canvas = null;
        [SerializeField] int rowsCount = 10;
        [SerializeField] int columnsCount = 10;
        [SerializeField] bool alignWithMousePosition = default;

        private void Start()
        {
            InitLootPanel();

        }

        void InitLootPanel()
        {
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
            if (alignWithMousePosition)
            {
                SetMainPanelPositionToMousePosition();
            }
        }

        private void SetMainPanelPositionToMousePosition()
        {
            float maxCanvasWidth = canvas.GetComponent<RectTransform>().rect.width - mainPanel.rect.width;
            float maxCanvasHeight = canvas.GetComponent<RectTransform>().rect.height - mainPanel.rect.height;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 lerp = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            mainPanel.anchoredPosition = new Vector2(
                Mathf.Lerp(0f, maxCanvasWidth, lerp.x),
                -Mathf.Lerp(0f, maxCanvasHeight, 1 - lerp.y));
        }

        private void PlaceSlots(RectTransform slotRect)
        {
            for (int row = 1; row <= rowsCount; row++)
            {
                float anchoredY = slotRect.anchoredPosition.y * row - slotRect.rect.height * (row - 1);
                for (int column = 1; column <= columnsCount; column++)
                {
                    float anchoredX = slotRect.anchoredPosition.x * column + slotRect.rect.width * (column - 1);
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    slotInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredX, anchoredY);
                }
            }
        }

        private void ComputePanelSize(RectTransform slotRect)
        {
            float marginX = Mathf.Abs(slotsPanel.sizeDelta.x);
            float marginY = Mathf.Abs(slotsPanel.sizeDelta.y);
            float w = (slotRect.rect.width + Mathf.Abs(slotRect.anchoredPosition.x)) * columnsCount + marginX;
            float h = (slotRect.rect.height + Mathf.Abs(slotRect.anchoredPosition.y)) * rowsCount + marginY;
            mainPanel.sizeDelta = new Vector2(w, h);
            mainPanel.anchoredPosition = Vector2.zero;
        }
    }
}
