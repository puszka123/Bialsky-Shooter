using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.EquipmentSystem
{
    public class EquipmentItemSelection : MonoBehaviour
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemCleared;
        public Guid itemId;
        RectTransform rect;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            InitInputSystem();
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
    }
}
