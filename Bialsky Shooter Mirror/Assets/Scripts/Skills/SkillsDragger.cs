using BialskyShooter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BialskyShooter.SkillSystem
{
    [RequireComponent(typeof(SkillsDisplay))]
    public class SkillsDragger : MonoBehaviour
    {
        SkillsDisplay skillsDisplay;

        private void Start()
        {
            skillsDisplay = GetComponent<SkillsDisplay>();
            Draggable.clientOnEndDrag += OnEndDrag;
        }

        private void OnDestroy()
        {
            Draggable.clientOnEndDrag -= OnEndDrag;
        }

        private void OnEndDrag(Draggable draggable)
        {
            var draggedSkillSlot = draggable.GetComponent<ISkillSlot>();
            if (draggedSkillSlot == null) return;
            GameObject injectedSkillSlot = default;
            foreach (var skillSlot in skillsDisplay.SkillsSlots)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    skillSlot.GetComponent<RectTransform>(),
                    Mouse.current.position.ReadValue()))
                {
                    skillSlot.GetComponent<SkillSlot>()
                        .InjectSkill(draggedSkillSlot.GetSkillId(), draggedSkillSlot.GetSkillIcon());
                    injectedSkillSlot = skillSlot;
                    break;
                }
            }
            if(draggable.gameObject != injectedSkillSlot) draggedSkillSlot.RemoveSkill();
        }
    }
}
