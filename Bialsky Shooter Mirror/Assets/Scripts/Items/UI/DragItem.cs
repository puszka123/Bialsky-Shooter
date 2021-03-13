using BialskyShooter.EquipmentSystem.UI;
using BialskyShooter.InventoryModule.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        IItemSlot draggingItemSlot;
        DragItemMock dragItemMockPrefab;
        DragItemMock dragItemMockInstance;


        public void OnBeginDrag(PointerEventData eventData)
        {
            draggingItemSlot = GetComponent<IItemSlot>();
            if (draggingItemSlot == null
                || draggingItemSlot.GetItemInformation() == null
                || draggingItemSlot.ReadyOnly()) return;
            draggingItemSlot.SetItemVisibility(false);
            dragItemMockPrefab = Resources.Load<DragItemMock>("DragItemMock");
            dragItemMockInstance = Instantiate(dragItemMockPrefab);
            dragItemMockInstance.SetSprite(Resources.Load<Sprite>(draggingItemSlot.GetItemInformation().iconPath));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragItemMockInstance != null)
            {
                dragItemMockInstance.SetPosition(Mouse.current.position.ReadValue());
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (draggingItemSlot == null
                || draggingItemSlot.GetItemInformation() == null
                || draggingItemSlot.ReadyOnly()) return;
            draggingItemSlot.SetItemVisibility(true);
            var itemSlot = TryToGet<IItemSlot>(eventData);
            var isDragged = DragToSlot(itemSlot);
            if (!isDragged) isDragged = DragToEquipmentContainer(eventData);
            if (!isDragged) isDragged = DragToInventoryContainer(eventData);
            if (!isDragged) DropItem();
            Destroy(dragItemMockInstance.gameObject);
        }

        private bool DragToEquipmentContainer(PointerEventData eventData)
        {
            var equipmentSlotsContainer = TryToGet<IEquipmentSlotsContainer>(eventData);
            if (equipmentSlotsContainer != null)
            {
                DragItemTo(equipmentSlotsContainer);
                return true;
            }
            return false;
        }

        private bool DragToInventoryContainer(PointerEventData eventData)
        {
            var inventorySlotsContainer = TryToGet<IInventorySlotsContainer>(eventData);
            if (inventorySlotsContainer != null)
            {
                DragItemTo(inventorySlotsContainer);
                return true;
            }
            return false;
        }

        private bool DragToSlot(IItemSlot itemSlot)
        {
            if (itemSlot != null)
            {
                DragItemTo(itemSlot);
                return true;
            }
            return false;
        }

        T TryToGet<T>(PointerEventData eventData)
        {
            foreach (var graphicRaycaster in FindObjectsOfType<GraphicRaycaster>())
            {
                var raycastResults = new List<RaycastResult>();
                graphicRaycaster.Raycast(eventData, raycastResults);
                var uiElement = raycastResults
                    .Where(ray => ray.gameObject.GetComponent<T>() != null)
                    .Select(ray => ray.gameObject.GetComponent<T>())
                    .FirstOrDefault();
                if (uiElement != null) return uiElement;
            }
            return default;
        }

        void DragItemTo(IItemSlot itemSlot)
        {
            var result = ItemStacker.Instance.TryStackItems(draggingItemSlot, itemSlot);
            if (!result) ItemSwapper.SwapItems(draggingItemSlot, itemSlot);
        }

        void DragItemTo(IEquipmentSlotsContainer container)
        {
            container.InjectItem(draggingItemSlot);
            if (draggingItemSlot is EquipmentItemSlot) return;
            draggingItemSlot.DragOutItem();
        }

        void DragItemTo(IInventorySlotsContainer container)
        {
            ItemSwapper.SwapItems(draggingItemSlot, container);
        }

        void DropItem()
        {
            draggingItemSlot.DragOutItem();
        }
    }
}
