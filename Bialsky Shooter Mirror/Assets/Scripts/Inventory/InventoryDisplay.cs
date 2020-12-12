using BialskyShooter.ItemSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.InventoryModule
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform slotsPanel = null;
        [SerializeField] RectTransform mainPanel = null;
        [SerializeField] GameObject slotImagePrefab = null;
        [SerializeField] int rowsCount = 10;
        [SerializeField] int columnsCount = 10;
        GameObject[] slots;
        Dictionary<GameObject, bool> slotsAvailability;

        private void Start()
        {
            Display();
            InventoryItemSelection.clientOnItemSelected += OnItemSelected;
            InventoryItemSelection.clientOnItemCleared += OnItemCleared;
            InventoryItemSelection.clientOnItemInjected += OnItemInjected;
        }

        private void OnDestroy()
        {
            InventoryItemSelection.clientOnItemSelected -= OnItemSelected;
            InventoryItemSelection.clientOnItemCleared -= OnItemCleared;
            InventoryItemSelection.clientOnItemInjected -= OnItemInjected;
        }

        private void OnItemSelected(Guid itemId)
        {
            SetSlotAvailability(itemId, true);
        }

        private void OnItemCleared(Guid itemId)
        {
            SetSlotAvailability(itemId, true);
        }

        private void OnItemInjected(Guid itemId)
        {
            SetSlotAvailability(itemId, false);
        }

        private void SetSlotAvailability(Guid itemId, bool availability)
        {
            GameObject slot = slotsAvailability.Keys
                            .FirstOrDefault(s => s.GetComponent<InventoryItemSelection>().itemId == itemId);
            if (slot == null) return;
            slotsAvailability[slot] = availability;
        }

        void InitLootPanel()
        {
            var slotRect = slotImagePrefab.GetComponent<RectTransform>();
            ComputePanelSize(slotRect);
            PlaceSlots(slotRect);
        }

        public void Display()
        {
            slots = new GameObject[rowsCount * columnsCount];
            slotsAvailability = new Dictionary<GameObject, bool>();
            InitLootPanel();
        }

        private void PlaceSlots(RectTransform slotRect)
        {
            int index = 0;
            for (int row = 1; row <= rowsCount; row++)
            {
                float anchoredY = slotRect.anchoredPosition.y * row - slotRect.rect.height * (row - 1);
                for (int column = 1; column <= columnsCount; column++)
                {
                    float anchoredX = slotRect.anchoredPosition.x * column + slotRect.rect.width * (column - 1);
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    slotInstance.AddComponent<InventoryItemSelection>();
                    InitSlots(index, slotInstance);
                    slotInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredX, anchoredY);
                    ++index;
                }
            }
        }

        private void InitSlots(int index, GameObject slotInstance)
        {
            if (slots == null) return;
            slots[index] = slotInstance;
            slotsAvailability[slotInstance] = true;
        }

        private void SetInventoryItemSelection(GameObject slotInstance, Guid itemId)
        {
            var slotItemSelection = slotInstance.GetComponent<InventoryItemSelection>();
            slotItemSelection.itemId = itemId;
        }

        private void DisplayItem(GameObject slotInstance, Sprite icon)
        {
            var image = slotInstance.transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = icon;
        }

        public void DisplayItem(ItemDisplay displayItem)
        {
            if (ItemExists(displayItem)) return;
            var slot = slotsAvailability.First(pair => pair.Value).Key;
            DisplayItem(slot, displayItem.Icon);
            SetInventoryItemSelection(slot, displayItem.ItemId);
            slotsAvailability[slot] = false;
        }

        private bool ItemExists(ItemDisplay displayItem)
        {
            return slots
                .Select(s => s.GetComponent<InventoryItemSelection>().itemId)
                .Contains(displayItem.ItemId);
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