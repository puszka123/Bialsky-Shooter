using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Barrier", menuName = "ScriptableObjects/Skills/Barrier")]
    public class Barrier : Skill
    {
        public override void Use(ISkillUser skillUser)
        {
            foreach (var barrier in barriers)
            {
                skillUser.ReceiveBarrier(barrier.Create());
            }
        }
    }
}