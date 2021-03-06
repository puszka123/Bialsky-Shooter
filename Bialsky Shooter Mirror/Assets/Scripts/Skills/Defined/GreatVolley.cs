using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Great Volley", menuName = "ScriptableObjects/Skills/Great Volley")]
    public class GreatVolley : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            foreach (var buff in buffs)
            {
                skillUser.ReceiveBuff(buff.Create());
            }
            skillUser.ResetWeapon();
        }
    }
}
