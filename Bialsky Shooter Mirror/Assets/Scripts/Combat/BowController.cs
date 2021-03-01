using BialskyShooter.EnhancementsModule;
using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public class BowController : MonoBehaviour, IWeaponController
    {
        [SerializeField] GameObject projectilePrefab;
        GameObject user;
        EnhancementSender userEnhancementSender;
        NetworkIdentity userNetworkIdentity;
        IEnumerable<AttackEnhancement> enhancements;
        bool inProgress;
        IWeapon weapon;

        #region Server

        public void StartControl(GameObject user, IWeapon weapon)
        {
            userEnhancementSender = user.GetComponent<EnhancementSender>();
            userNetworkIdentity = user.GetComponent<NetworkIdentity>();
            GetAttackEnhancements();
            this.weapon = weapon;
            if (!inProgress)
            {
                StartCoroutine(ControlInProgress());
            }
        }

        [Server]
        void GetAttackEnhancements()
        {
            enhancements = userEnhancementSender.Get(AttackType.Weapon);
        }

        [Server]
        float GetAdditiveEnhancement()
        {
            var enhancement = 0f;
            foreach (var item in enhancements.Where(e => !e.used))
            {
                enhancement += item.value;
                item.used = true;
            }
            return enhancement;
        }

        [Server]
        float GetPercentageEnhancement()
        {
            var enhancement = 0f;
            foreach (var item in enhancements.Where(e => !e.used))
            {
                enhancement += item.percentageValue;
                item.used = true;
            }
            return enhancement / 100f;
        }

        [Server]
        IEnumerator ControlInProgress()
        {
            inProgress = true;
            Fire();
            yield return new WaitForFixedUpdate();
            inProgress = false;
            StopControl();
        }

        [Server]
        private void Fire()
        {
            var damage = weapon.GetDamage() + (weapon.GetDamage() * GetPercentageEnhancement()) + GetAdditiveEnhancement();
            var projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectileInstance.GetComponent<Projectile>().Init(userNetworkIdentity, damage);
            NetworkServer.Spawn(projectileInstance);
        }

        [Server]
        private void StopControl()
        {
            enhancements = null;
            userEnhancementSender = null;
            user = null;
            userNetworkIdentity = null;
        }

        #endregion
    }
}