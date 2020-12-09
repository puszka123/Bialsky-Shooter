using BialskyShooter.ItemSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule
{
    public class LootDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] Canvas canvas = null;
        int rowsCount;
        int columnsCount;
        List<ItemDisplay> itemDisplays;

        public void Display(IEnumerable<ItemDisplay> itemDisplays)
        {
            if (itemDisplays == null) return;
            this.itemDisplays = new List<ItemDisplay>(itemDisplays);
            if (this.itemDisplays.Count == 0) return;
            var count = this.itemDisplays.Count;
            rowsCount = count / 2;
            columnsCount = count / 2;
            rowsCount += count % 2;
            if (columnsCount == 0) columnsCount = 1;
            if (rowsCount == 0) rowsCount = 1;
            if(count == 2)
            {
                rowsCount = 1;
                columnsCount = 2;
            }
            InitLootPanel();
            SetMainPanelPositionToMousePosition();
        }

        void InitLootPanel()
        {
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
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
                    DisplayItem(slotInstance, itemDisplays[row + column - 2].Icon);
                    SetLootItemSelection(slotInstance, itemDisplays[row + column - 2].ItemId);
                }
            }
        }

        private void SetLootItemSelection(GameObject slotInstance, Guid itemId)
        {
            var slotItemSelection = slotInstance.GetComponent<LootItemSelection>();
            slotItemSelection.itemId = itemId;
        }

        private void DisplayItem(GameObject slotInstance, Sprite icon)
        {
            var image = slotInstance.transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = icon;
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
