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
        SkillDisplayData skill;

        public SkillDisplayData Skill { get { return skill; } }

        public Sprite GetSkillIcon()
        {
            return Resources.Load<Sprite>(Skill.iconPath);
        }

        public Guid GetSkillId()
        {
            return Skill.id;
        }

        public void InjectSkill(ISkillSlot skillSlot) { }

        public void RemoveSkill() { }

        public void SetSkill(SkillDisplayData skill)
        {
            this.skill = skill;
            DisplaySkill(skill);
        }

        void DisplaySkill(SkillDisplayData skill)
        {
            skillImage.sprite = Resources.Load<Sprite>(skill.iconPath);
            skillImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
