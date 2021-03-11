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
    public class EquipmentItemSlot : MonoBehaviour, IItemSlot
    {
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemDraggedIn;
        public static event Action<Guid> clientOnItemDraggedOut;

        [SerializeField] ItemSlotType itemSlotType = default;
        ItemInformation itemInformation;
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
            if (itemInformation == null) return;
            clientOnItemSelected?.Invoke(itemInformation.ItemId);
        }

        public void InjectItem(ItemInformation itemInformation)
        {
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
            this.itemInformation = itemInformation;
        }

        public Guid ClearItem()
        {
            var clearedItemId = itemInformation?.ItemId ?? Guid.Empty;
            var image = transform.GetChild(0).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            itemInformation = null;
            return clearedItemId;
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void DragInItem(IItemSlot itemSlot)
        {
            if (readOnlyMode) return;
            clientOnItemDraggedIn?.Invoke(itemSlot.GetItemId());
        }

        public Guid DragOutItem()
        {
            if (readOnlyMode) return Guid.Empty;
            var itemId = ClearItem();
            clientOnItemDraggedOut?.Invoke(itemId);
            return itemId;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, visibility ? 1 : 0);
        }

        public ItemInformation GetItemInformation()
        {
            return itemInformation;
        }

        public Guid GetItemId()
        {
            return itemInformation?.ItemId ?? Guid.Empty;
        }
    }
}
