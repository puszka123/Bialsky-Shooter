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
        List<ItemDisplay> itemDisplays;

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
            var slot = SetSlotAvailability(itemId, true);
        }

        private void OnItemCleared(Guid itemId)
        {
            var slot = SetSlotAvailability(itemId, true);
        }

        private void OnItemInjected(Guid itemId)
        {
            var slot = SetSlotAvailability(itemId, false);
            SetItemInformationToggle(slot, itemDisplays.FirstOrDefault(e => e.ItemId == itemId));
        }

        private GameObject SetSlotAvailability(Guid itemId, bool availability)
        {
            GameObject slot = slotsAvailability.Keys
                            .FirstOrDefault(s => s.GetComponent<InventoryItemSelection>().itemId == itemId);
            if (slot == null) return null;
            slotsAvailability[slot] = availability;
            return slot;
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
            itemDisplays = new List<ItemDisplay>();
            InitLootPanel();
        }

        private void PlaceSlots(RectTransform slotRect)
        {
            int index = 0;
            for (int row = 1; row <= rowsCount; row++)
            {
                for (int column = 1; column <= columnsCount; column++)
                {
                    var slotInstance = Instantiate(slotImagePrefab, slotsPanel);
                    slotInstance.AddComponent<InventoryItemSelection>();
                    InitSlots(index, slotInstance);
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

        private void SetItemInformationToggle(GameObject slot, ItemDisplay displayItem)
        {
            if (slot == null || slot.GetComponent<ItemInformationToggle>() == null) return;
            slot.GetComponent<ItemInformationToggle>().SetItemDisplay(displayItem);
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
            SetItemInformationToggle(slot, displayItem);
            slotsAvailability[slot] = false;
            itemDisplays.Add(displayItem);
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