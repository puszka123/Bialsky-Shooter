using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BialskyShooter.SkillSystem
{
    [System.Serializable]
    public class SkillsBook
    {
        public List<Skill> availableSkills;
        Dictionary<string, Skill> skillBindings;

        public SkillsBook()
        {
            skillBindings = new Dictionary<string, Skill>();
        }

        public Skill GetSkill(Guid skillId)
        {
            return availableSkills.First(s => s.Id == skillId);
        }

        public Skill GetSkill(string keyBinding)
        {
            return skillBindings[keyBinding];
        }

        public Skill GetSkill(int index)
        {
            return availableSkills[index];
        }

        public void BindSkill(string keyBinding, Skill skill)
        {
            skillBindings[keyBinding] = skill;
        }

        public void BindSkill(string keyBinding, Guid skillId)
        {
            skillBindings[keyBinding] = GetSkill(skillId);
        }
    }
}
