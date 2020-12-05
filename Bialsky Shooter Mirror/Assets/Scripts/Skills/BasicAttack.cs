using BialskyShooter.ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "ScriptableObjects/Skills/BasicAttack")]
    public class BasicAttack : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            Debug.Log(skillUser);
            skillUser.UseWeapon();
        }
    }
}
