using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Teleport", menuName = "ScriptableObjects/Skills/Teleport")]
    public class Teleport : Skill
    {
        [SerializeField] float distance = 100f;
        public override void Use(ISkillUser skillUser)
        {
            skillUser.GetMovement().TargetTeleport(skillUser.GetMouseWorldPosition());
            skillUser.GetMovement().Teleport(skillUser.GetMouseWorldPosition());
        }
    }
}
