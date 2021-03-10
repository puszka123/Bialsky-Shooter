using BialskyShooter.Combat;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    [CreateAssetMenu(fileName = "EnergyBall", menuName = "ScriptableObjects/Skills/EnergyBall")]
    public class EnergyBall : Skill
    {
        [SerializeField] GameObject energyBallPrefab;
        [SerializeField] float damage;
        public override void Use(ISkillUser skillUser)
        {
            var weaponTransform = skillUser.GetWeaponTransform();
            var userIdentity = skillUser.GetTransform().GetComponent<NetworkIdentity>();
            var projectileInstance = Instantiate(energyBallPrefab, weaponTransform.position, weaponTransform.rotation);
            projectileInstance.GetComponent<Projectile>().Init(userIdentity, damage);
            NetworkServer.Spawn(projectileInstance);
        }
    }
}
