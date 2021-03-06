using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Basic Defence", menuName = "ScriptableObjects/Skills/Basic Defence")]
    public class BasicDefence : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            foreach (var buff in buffs)
            {
                skillUser.ReceiveBuff(buff.Create());
            }
            skillUser.UseWeapon(false);
        }
    }
}
