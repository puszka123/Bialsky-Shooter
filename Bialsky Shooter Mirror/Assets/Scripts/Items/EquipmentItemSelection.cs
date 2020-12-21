using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem
{
    public class EquipmentItemSelection : MonoBehaviour, IItemSelection
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemCleared;
        public static event Action<Guid> clientOnItemDraggedIn;
        public Guid itemId;
        RectTransform rect;
        Controls controls;
        bool readOnlyMode;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            if (!readOnlyMode)
            {
                InitInputSystem();
                Draggable.clientOnEndDrag += OnEndDrag;
            }
        }

        private void OnDestroy()
        {
            if (!readOnlyMode) Draggable.clientOnEndDrag -= OnEndDrag;
        }

        public void ReadOnly()
        {
            readOnlyMode = true;
        }

        private void InitInputSystem()
        {
            controls = new Controls();
            controls.Player.Inventory.performed += EquipmentPerformed;
            controls.Enable();
        }

        private void EquipmentPerformed(InputAction.CallbackContext ctx)
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

        public Guid GetItemId()
        {
            return itemId;
        }

        public Image GetItemImage()
        {
            return transform.GetChild(0).GetComponent<Image>();
        }

        public void ItemDragged()
        {
            if (readOnlyMode) return;
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        private void OnEndDrag(Draggable draggable)
        {
            if (readOnlyMode) return;
            if (!draggable.TryGetComponent(out IItemSelection itemSelection)) return;
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return;
            if (itemSelection.GetItemId() == Guid.Empty) return;
            clientOnItemDraggedIn?.Invoke(itemSelection.GetItemId());
            itemSelection.ItemDragged();
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }
    }
}
