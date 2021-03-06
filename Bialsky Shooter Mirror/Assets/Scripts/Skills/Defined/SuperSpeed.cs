using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "SuperSpeed", menuName = "ScriptableObjects/Skills/SuperSpeed")]
    public class SuperSpeed : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            foreach (var buff in buffs)
            {
                skillUser.ReceiveBuff(buff.Create());
            }
        }
    }
}
