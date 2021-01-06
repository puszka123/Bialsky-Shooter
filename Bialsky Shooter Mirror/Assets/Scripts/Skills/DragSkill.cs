using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace BialskyShooter.SkillSystem
{
    public class DragSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        ISkillSlot draggingSkillSlot;

        public void OnBeginDrag(PointerEventData eventData)
        {
            draggingSkillSlot = GetComponent<ISkillSlot>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var itemSlot = TryToGetSkillSlot(eventData);
            if (itemSlot != null)
            {
                DragItemTo(itemSlot);
            }
            else
            {
                DropItem();
            }
        }

        ISkillSlot TryToGetSkillSlot(PointerEventData eventData)
        {
            foreach (var graphicRaycaster in FindObjectsOfType<GraphicRaycaster>())
            {
                var raycastResults = new List<RaycastResult>();
                graphicRaycaster.Raycast(eventData, raycastResults);
                var itemSlot = raycastResults
                    .Where(ray => ray.gameObject.GetComponent<ISkillSlot>() != null)
                    .Select(ray => ray.gameObject.GetComponent<ISkillSlot>())
                    .FirstOrDefault();
                if (itemSlot != null) return itemSlot;
            }
            return null;
        }

        void DragItemTo(ISkillSlot itemSlot)
        {
            itemSlot.InjectSkill(draggingSkillSlot);
            draggingSkillSlot.RemoveSkill();
        }

        void DropItem()
        {
            draggingSkillSlot.RemoveSkill();
        }
    }
}
