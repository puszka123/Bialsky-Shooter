using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "PowerfulAttack", menuName = "ScriptableObjects/Skills/PowerfulAttack")]
    public class PowerfulAttack : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            foreach (var enhancement in attackEnhancements)
            {
                skillUser.ReceiveAttackEnhancement(enhancement.Create());
            }
        }
    }
}