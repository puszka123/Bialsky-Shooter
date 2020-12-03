using BialskyShooter.Combat;
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
            var weapon = skillUser.GetWeapon();
            var transform = skillUser.GetTransform();
            Debug.DrawRay(transform.position,
                transform.forward, Color.red, 10f);
            if (Physics.Raycast(transform.position + Vector3.up/2, 
                transform.forward, 
                out RaycastHit hit, 
                weapon.Stats.range.value,
                layerMask
                ))
            {
                Debug.DrawRay(transform.position,
                transform.forward, Color.blue, 10f);
                OnHit(hit.transform.GetComponent<CombatTarget>()?.GetComponent<Health>(), weapon);
            }
        }

        void OnHit(Health target, Weapon weapon)
        {
            target.TakeDamage(weapon.Stats.damage.value);
        }
    }
}
