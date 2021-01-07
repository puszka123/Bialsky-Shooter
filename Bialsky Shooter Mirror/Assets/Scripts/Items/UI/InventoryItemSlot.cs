using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public class InventoryItemSlot : MonoBehaviour, IItemSlot
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemCleared;
        public static event Action<Guid> clientOnItemInjected;
        public Guid itemId;
        RectTransform rect;
        bool readOnlyMode = default;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            InitInputSystem();
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.Inventory.performed += InventoryPerformed;
            controls.Enable();
        }

        private void InventoryPerformed(InputAction.CallbackContext ctx)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return;
            if (this.itemId == Guid.Empty) return;
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        public Guid ClearItem()
        {
            var clearedItemId = itemId;
            clientOnItemCleared?.Invoke(clearedItemId);
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            itemId = Guid.Empty;
            return clearedItemId;
        }

        void InjectItem(Guid itemId, Sprite icon)
        {
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = icon;
            this.itemId = itemId;
            clientOnItemInjected?.Invoke(itemId);
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            InjectItem(itemSlot.GetItemId(), itemSlot.GetItemIcon());
        }

        public Guid GetItemId()
        {
            return itemId;
        }

        public Sprite GetItemIcon()
        {
            return transform.GetChild(0).GetComponent<Image>().sprite;
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,visibility ? 1 : 0);
        }
    }
}