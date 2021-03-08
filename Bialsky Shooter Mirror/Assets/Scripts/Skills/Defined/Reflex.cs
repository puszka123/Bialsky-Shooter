using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Reflex", menuName = "ScriptableObjects/Skills/Reflex")]
    public class Reflex : Skill
    {
        [SerializeField] int dodgeCount = 4;
        public override void Use(ISkillUser skillUser)
        {
            foreach (var buff in buffs)
            {
                skillUser.ReceiveBuff(buff.Create());
            }
        }

        public override int GetUsesCount()
        {
            return dodgeCount;
        }

        public override float UseInterval()
        {
            foreach (var buff in buffs)
            {
                return buff.duration;
            }
            return 0;
        }
    }
}
