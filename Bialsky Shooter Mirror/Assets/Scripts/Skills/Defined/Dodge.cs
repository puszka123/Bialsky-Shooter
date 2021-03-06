using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "Dodge", menuName = "ScriptableObjects/Skills/Dodge")]
    public class Dodge : Skill
    {
        [SerializeField] float moveTime = 0.5f;
        [SerializeField] float moveForce = 2f;
        public override void Use(ISkillUser skillUser)
        {
            skillUser.ExecuteCoroutine(MoveBackward(skillUser, moveTime));
        }

        IEnumerator MoveBackward(ISkillUser skillUser, float time)
        {
            while (time > 0f)
            {
                yield return new WaitForFixedUpdate();
                time -= Time.fixedDeltaTime;
                skillUser.GetMovement().TargetMove(-skillUser.GetTransform().forward * moveForce, Time.fixedDeltaTime);
                skillUser.GetMovement().Move(-skillUser.GetTransform().forward * moveForce, Time.fixedDeltaTime);
            }
        }
    }
}
