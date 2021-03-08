using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "FlashySeries", menuName = "ScriptableObjects/Skills/FlashySeries")]
    public class FlashySeries : Skill
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