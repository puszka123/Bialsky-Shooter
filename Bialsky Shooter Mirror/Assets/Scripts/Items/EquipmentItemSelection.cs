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

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            InitInputSystem();
            Draggable.clientOnEndDrag += OnEndDrag;
        }

        private void OnDestroy()
        {
            Draggable.clientOnEndDrag -= OnEndDrag;
        }

        private void InitInputSystem()
        {
            Controls controls = new Controls();
            controls.Player.Inventory.performed += EquipmentPerformed;
            controls.Enable();
        }

        private void EquipmentPerformed(InputAction.CallbackContext ctx)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return;
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        private Guid ClearItem()
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
            var itemId = ClearItem();
            clientOnItemSelected?.Invoke(itemId);
        }

        private void OnEndDrag(Draggable draggable)
        {
            if (!draggable.TryGetComponent<IItemSelection>(out IItemSelection itemSelection)) return;
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, Mouse.current.position.ReadValue())) return;
            clientOnItemDraggedIn?.Invoke(itemSelection.GetItemId());
            itemSelection.ItemDragged();
        }
    }
}
