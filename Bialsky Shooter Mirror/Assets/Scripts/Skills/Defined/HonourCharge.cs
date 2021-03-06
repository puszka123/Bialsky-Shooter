using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "HonourCharge", menuName = "ScriptableObjects/Skills/HonourCharge")]
    public class HonourCharge : Skill
    {
        [SerializeField] float moveForce;
        public override void Use(ISkillUser skillUser)
        {
            skillUser.GetMovement().MoveForce(skillUser.GetTransform().forward * moveForce);
        }
    }
}
