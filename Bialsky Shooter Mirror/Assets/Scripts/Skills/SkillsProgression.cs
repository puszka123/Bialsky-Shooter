using BialskyShooter.ClassSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "SkillsProgression", menuName = "ScriptableObjects/Skills/SkillsProgression")]
    public class SkillsProgression : ScriptableObject
    {
        public ProgressionCreatureSkill[] creatureClasses;
        Dictionary<ClassType, Dictionary<int, Skill[]>> skillProgressionBook;

        public IEnumerable<Skill> GetAvailableSkills(ClassType classType, int level)
        {
            if (skillProgressionBook == null) InitSkillProgressionBook();
            var skills = new List<Skill>();
            for (int i = 0; i < level; i++)
            {
                skills.AddRange(skillProgressionBook[classType][i]);
            }
            return skills;
        }

        private void InitSkillProgressionBook()
        {
            skillProgressionBook = new Dictionary<ClassType, Dictionary<int, Skill[]>>();
            foreach (var creatureClass in creatureClasses)
            {
                var skillsPerLevel = new Dictionary<int, Skill[]>();
                for (int i = 0; i < creatureClass.progressionSkills.Length; i++)
                {
                    skillsPerLevel[i] = creatureClass.progressionSkills[i].skills;
                }
                skillProgressionBook[creatureClass.classType] = skillsPerLevel;
            }
        }
    }
}
