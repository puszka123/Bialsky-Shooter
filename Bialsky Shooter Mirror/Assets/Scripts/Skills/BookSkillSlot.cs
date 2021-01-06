using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.SkillSystem
{
    public class BookSkillSlot : MonoBehaviour, ISkillSlot
    {
        [SerializeField] Image skillImage = default;
        Skill skill;

        public Skill Skill { get { return skill; } }

        public Sprite GetSkillIcon()
        {
            return Skill.Icon;
        }

        public Guid GetSkillId()
        {
            return Skill.Id;
        }

        public void InjectSkill(ISkillSlot skillSlot) { }

        public void RemoveSkill() { }

        public void SetSkill(Skill skill)
        {
            this.skill = skill;
            DisplaySkill(skill);
        }

        void DisplaySkill(Skill skill)
        {
            skillImage.sprite = skill.Icon;
            skillImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
