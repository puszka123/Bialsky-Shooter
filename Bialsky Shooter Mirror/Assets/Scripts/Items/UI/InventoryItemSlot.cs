using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace BialskyShooter.ItemSystem.UI
{
    public class InventoryItemSlot : MonoBehaviour, IItemSlot
    {
        [Inject] ItemCountDisplay itemCountDisplay;
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemDraggedIn;
        public static event Action<Guid> clientOnItemDraggedOut;
        public ItemInformation itemInformation;
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
            if (itemInformation == null) return;
            clientOnItemSelected?.Invoke(itemInformation.ItemId);
        }

        public Guid ClearItem()
        {
            if (readOnlyMode) return Guid.Empty;
            var clearedItemId = itemInformation?.ItemId ?? Guid.Empty;
            var image = transform.GetChild(1).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.sprite = null;
            itemInformation = null;
            GetComponent<ItemCountDisplay>().Disable();
            return clearedItemId;
        }

        public void InjectItem(ItemInformation itemInformation)
        {
            if (readOnlyMode) return;
            var image = transform.GetChild(1).GetComponent<Image>();
            image.color = new Color(1, 1, 1, 1);
            image.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
            this.itemInformation = itemInformation;
            if (itemInformation.stackable)
            {
                GetComponent<ItemCountDisplay>().SetCount(itemInformation.count);
            }
        }

        public void DragInItem(IItemSlot itemSlot)
        {
            InjectItem(itemSlot.GetItemInformation());
            clientOnItemDraggedIn?.Invoke(itemSlot.GetItemId());
        }

        public Guid DragOutItem()
        {
            var itemId = ClearItem();
            clientOnItemDraggedOut?.Invoke(itemId);
            return itemId;
        }

        public bool ReadyOnly()
        {
            return readOnlyMode;
        }

        public void SetItemVisibility(bool visibility)
        {
            transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,visibility ? 1 : 0);
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
