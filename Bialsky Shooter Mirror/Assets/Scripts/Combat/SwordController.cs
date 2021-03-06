using BialskyShooter.EnhancementsModule;
using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(BoxCollider))]
    public class SwordController : MonoBehaviour, IWeaponController
    {
        GameObject user;
        EnhancementSender userEnhancementSender;
        NetworkIdentity userNetworkIdentity;
        IEnumerable<AttackEnhancement> enhancements;
        bool inProgress;
        bool attack;
        float defenceTimer = 0f;
        IWeapon weapon;
        public Action<bool> OnStartControl { get; set; }
        public Action OnStopControl { get; set; }

        #region Server

        [ServerCallback]
        private void OnTriggerStay(Collider other)
        {
            if (!attack
                || other.gameObject == user
                || !inProgress
                || !other.TryGetComponent(out CombatTarget target)
                || target.Health.IsDefeated) return;
            var damage = weapon.GetDamage() + (weapon.GetDamage() * GetPercentageEnhancement()) + GetAdditiveEnhancement();
            target.Health.TakeDamage(userNetworkIdentity, damage);
        }

        [ServerCallback]
        private void Update()
        {
            if (!inProgress) return;
            defenceTimer -= Time.fixedDeltaTime;
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
        public void ResetDefenceTimer()
        {
            defenceTimer = weapon.GetCooldown();
        }

        [Server]
        public void StartControl(GameObject user, IWeapon weapon, bool attack = true)
        {
            userEnhancementSender = user.GetComponent<EnhancementSender>();
            userNetworkIdentity = user.GetComponent<NetworkIdentity>();
            GetAttackEnhancements();
            this.weapon = weapon;
            this.attack = attack;
            if (!inProgress)
            {
                StartCoroutine(ControlInProgress());
            }
        }

        [Server]
        IEnumerator ControlInProgress()
        {
            if(attack) yield return ControlAttack();
            else yield return ControlDefence();
        }

        [Server]
        IEnumerator ControlAttack()
        {
            OnStartControl?.Invoke(true);
            inProgress = true;
            yield return new WaitForSeconds(weapon.GetCooldown());
            inProgress = false;
            StopControl();
            OnStopControl?.Invoke();
        }

        [Server]
        IEnumerator ControlDefence()
        {
            OnStartControl?.Invoke(false);
            inProgress = true;
            defenceTimer = weapon.GetCooldown();
            yield return new WaitUntil(() => defenceTimer <= 0f);
            inProgress = false;
            StopControl();
            OnStopControl?.Invoke();
        }

        [Server]
        private void StopControl()
        {
            enhancements = null;
            userEnhancementSender = null;
            user = null;
            userNetworkIdentity = null;
        }

        [Server]
        public void Terminate()
        {
            StopAllCoroutines();
            inProgress = false;
            StopControl();
            OnStopControl?.Invoke();
        }

        #endregion
    }
}