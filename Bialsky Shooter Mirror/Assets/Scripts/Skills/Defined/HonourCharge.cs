using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Honour Charge", menuName = "ScriptableObjects/Skills/Honour Charge")]
    public class HonourCharge : Skill
    {
        [SerializeField] float moveForce;
        [SerializeField] float moveTime = 0.5f;
        public override void Use(ISkillUser skillUser)
        {
            skillUser.ExecuteCoroutine(MoveForward(skillUser, moveTime));
        }

        IEnumerator MoveForward(ISkillUser skillUser, float time)
        {
            while (time > 0f)
            {
                yield return new WaitForFixedUpdate();
                time -= Time.fixedDeltaTime;
                skillUser.GetMovement().TargetMove(skillUser.GetTransform().forward * moveForce, Time.fixedDeltaTime);
                skillUser.GetMovement().Move(skillUser.GetTransform().forward * moveForce, Time.fixedDeltaTime);
            }
        }
    }
}
