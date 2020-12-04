using BialskyShooter.ClassSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [System.Serializable]
    public class ProgressionCreatureSkill
    {
        public ClassType classType;
        public ProgressionSkill[] progressionSkills;
    }
}
