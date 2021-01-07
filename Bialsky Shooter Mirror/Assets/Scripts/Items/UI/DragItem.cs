using BialskyShooter.EquipmentSystem.UI;
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
            draggingItemSlot.GetItemImage().color = new Color(1, 1, 1, 0);
            dragItemMockPrefab = Resources.Load<DragItemMock>("DragItemMock");
            dragItemMockInstance = Instantiate(dragItemMockPrefab);
            dragItemMockInstance.SetSprite(draggingItemSlot.GetItemImage().sprite);
        }

        public void OnDrag(PointerEventData eventData)
        {;

            dragItemMockInstance.SetPosition(Mouse.current.position.ReadValue());
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            draggingItemSlot.GetItemImage().color = new Color(1, 1, 1, 1);
            var itemSlot = TryToGet<IItemSlot>(eventData);
            var isDragged = DragToSlot(itemSlot);
            if(!isDragged) isDragged = DragToEquipmentContainer(eventData);
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
            itemSlot.InjectItem(draggingItemSlot);
            draggingItemSlot.ClearItem();
        }

        void DragItemTo(IEquipmentSlotsContainer container)
        {
            container.InjectItem(draggingItemSlot);
            draggingItemSlot.ClearItem();
        }

        void DropItem()
        {
            draggingItemSlot.ClearItem();
        }
    }
}
