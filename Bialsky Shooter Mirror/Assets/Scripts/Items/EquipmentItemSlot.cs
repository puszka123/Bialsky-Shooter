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
    public class EquipmentItemSlot : MonoBehaviour, IItemSlot
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemCleared;
        public static event Action<Guid> clientOnItemDraggedIn;

        [SerializeField] ItemSlotType itemSlotType = default;
        public Guid itemId;
        RectTransform rect;
        Controls controls;
        bool readOnlyMode;

        public ItemSlotType ItemSlotType { get { return itemSlotType; } }

        private void Start()
        {
            rect = GetComponent<RectTransform>();
            if (!readOnlyMode)
            {
                InitInputSystem();
            }
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
            if (readOnlyMode) return Guid.Empty;

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

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            if (readOnlyMode) return;
            clientOnItemDraggedIn?.Invoke(itemSlot.GetItemId());
        }
    }
}
