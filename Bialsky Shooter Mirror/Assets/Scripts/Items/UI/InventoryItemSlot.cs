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
        public static event Action<Guid> clientOnItemSelected;
        public static event Action<Guid> clientOnItemDraggedIn;
        public static event Action<Guid> clientOnItemDraggedOut;
        public static event Action<Guid, Guid, int> clientOnItemStack;
        [SerializeField] Image itemImage;
        public ItemInformation ItemInformation { get; set; }
        RectTransform rect;
        bool readOnlyMode = default;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
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
            if (ItemInformation == null) return;
            clientOnItemSelected?.Invoke(ItemInformation.ItemId);
        }

        public Guid ClearItem()
        {
            if (readOnlyMode) return Guid.Empty;
            var clearedItemId = ItemInformation?.ItemId ?? Guid.Empty;
            itemImage.color = new Color(1, 1, 1, 0);
            itemImage.sprite = null;
            ItemInformation = null;
            GetComponent<ItemCountDisplay>().Disable();
            return clearedItemId;
        }

        public void InjectItem(ItemInformation itemInformation)
        {
            if (readOnlyMode) return;
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = Resources.Load<Sprite>(itemInformation.iconPath);
            this.ItemInformation = itemInformation;
            if (itemInformation.stackable)
            {
                GetComponent<ItemCountDisplay>().SetCount(itemInformation.count);
            }
        }

        public void UpdateItem(ItemInformation itemInformation)
        {
            if (itemInformation.stackable && itemInformation.count == 0) ClearItem();
            else InjectItem(itemInformation);
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
            itemImage.color = new Color(1,1,1,visibility ? 1 : 0);
        }

        public ItemInformation GetItemInformation()
        {
            return ItemInformation;
        }

        public Guid GetItemId()
        {
            return ItemInformation?.ItemId ?? Guid.Empty;
        }

        public void Stack(Guid sourceItemId, int count)
        {
            clientOnItemStack?.Invoke(sourceItemId, ItemInformation.ItemId, count);
        }
    }
}
