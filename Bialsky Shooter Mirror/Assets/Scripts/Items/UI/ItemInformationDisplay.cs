using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.ItemSystem.UI
{
    public class ItemInformationDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text itemName = default;
        [SerializeField] GameObject statDisplayPrefab = default;
        [SerializeField] RectTransform mainPanel = default;
        [SerializeField] RectTransform contentPanel;
        [SerializeField] RectTransform body = default;
        [SerializeField] Canvas canvas = default;
        ItemDisplay itemDisplay;
        RectTransform slotRect;

        private void Update()
        {
            if (!slotRect.gameObject.activeInHierarchy) Destroy(gameObject);
        }

        public void setItemDisplay(ItemDisplay itemDisplay, RectTransform slotRect)
        {
            this.itemDisplay = itemDisplay;
            this.slotRect = slotRect;
        }

        public void Display()
        {
            if (itemDisplay == null) return;
            SetMainPanelPositionToMousePosition();
            DisplayItemName(itemDisplay.ItemName);
            DisplayItemStats(itemDisplay.ItemStats);
        }

        void DisplayItemName(string name)
        {
            itemName.text = name;
        }

        private void DisplayItemStats(IEnumerable<ItemStat> stats)
        {
            if (stats == null) return;
            var statDisplayRect = statDisplayPrefab.GetComponent<RectTransform>();
            var anchoredX = statDisplayRect.anchoredPosition.x;
            int row = 1;
            foreach(var stat in stats)
            {
                float anchoredY = statDisplayRect.anchoredPosition.y * row - statDisplayRect.rect.height * (row - 1);
                var statDisplayInstance = Instantiate(statDisplayPrefab, body);
                statDisplayInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredX, anchoredY);
                SetStatDisplay(statDisplayInstance, stat);
                ++row;
            }
        }

        private void SetStatDisplay(GameObject statDisplayInstance, ItemStat stat)
        {
            var label = statDisplayInstance.transform.GetChild(0).GetComponent<TMP_Text>();
            var value = statDisplayInstance.transform.GetChild(1).GetComponent<TMP_Text>();
            label.text = stat.nameToDisplay;
            value.text = stat.value.ToString();
        }

        private void SetMainPanelPositionToMousePosition()
        {
            Vector3[] corners = new Vector3[4];
            slotRect.GetWorldCorners(corners);
            float maxCanvasWidth = canvas.GetComponent<RectTransform>().rect.width;
            float maxCanvasHeight = canvas.GetComponent<RectTransform>().rect.height;
            var slotPos = new Vector2(corners[0].x, corners[0].y);
            var lerp = new Vector2(slotPos.x / Screen.width, slotPos.y / Screen.height);
            var mainPanelFactor = new Vector2(mainPanel.rect.width / maxCanvasWidth, mainPanel.rect.height / maxCanvasHeight);
            var offset = Vector2.zero;
            if (lerp.x + mainPanelFactor.x > 1f)
            {
                offset.x = mainPanel.rect.width;  
            }
            if (1-lerp.y + mainPanelFactor.y > 1f)
            {
                offset.y = -mainPanel.rect.height;
                slotPos = new Vector2(corners[1].x, corners[1].y);
                lerp = new Vector2(slotPos.x / Screen.width, slotPos.y / Screen.height);
            }
            mainPanel.anchoredPosition = new Vector2(
                Mathf.Lerp(0f, maxCanvasWidth, lerp.x),
                -Mathf.Lerp(0f, maxCanvasHeight, 1 - lerp.y));
            mainPanel.anchoredPosition -= offset;
        }
    }
}
