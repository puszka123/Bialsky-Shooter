using BialskyShooter.ClassSystem;
using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(BoxCollider))]
    public class SwordController : WeaponController
    {
        [SerializeField] GameObject parent = default;
        bool inProgress;
        WeaponSO weapon;
        #region Server

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == parent 
                || !inProgress
                || !other.TryGetComponent<CombatTarget>(out CombatTarget target)
                || target.Health.IsDefeated) return;
            print(weapon);
            target.Health.TakeDamage(parent.GetComponent<NetworkIdentity>(), 
                weapon.Stats.GetStat(ItemStatType.Damage).value);
        }

        [Server]
        public override void StartControl(WeaponSO weapon)
        {
            this.weapon = weapon;
            if (!inProgress)
            {
                StartCoroutine(ControlInProgress());
            }
        }

        [Server]
        IEnumerator ControlInProgress()
        {
            inProgress = true;
            yield return new WaitForSeconds(0.5f);
            inProgress = false;
        }

        #endregion
    }
}