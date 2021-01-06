using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BialskyShooter.UI
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        IItemSlot draggingItemSlot;


        public void OnBeginDrag(PointerEventData eventData)
        {
            draggingItemSlot = GetComponent<IItemSlot>();
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var itemSlot = TryToGet<IItemSlot>(eventData);
            if (itemSlot != null)
            {
                DragItemTo(itemSlot);
                return;
            }
            var equipmentSlotsContainer = TryToGet<IEquipmentSlotsContainer>(eventData);
            if(equipmentSlotsContainer != null)
            {
                DragItemTo(equipmentSlotsContainer);
            }

            DropItem();
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
