using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BialskyShooter.SkillSystem
{
    public class SkillDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        Skill draggingSkill;

        public void SetDragSkill(Skill skill)
        {
            draggingSkill = skill;
        }

        public Skill GetDragSkill()
        {
            return draggingSkill;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            print("start drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            print("drag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            print("end drag");
        }
    }
}
