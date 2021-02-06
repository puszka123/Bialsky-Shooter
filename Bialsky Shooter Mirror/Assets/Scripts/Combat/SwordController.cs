using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(BoxCollider))]
    public class SwordController : NetworkBehaviour, WeaponController
    {
        [SerializeField] GameObject parent = default;
        bool inProgress;
        IWeapon weapon;

        #region Server

        [ServerCallback]
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == parent 
                || !inProgress
                || !other.TryGetComponent(out CombatTarget target)
                || target.Health.IsDefeated) return;
            target.Health.TakeDamage(parent.GetComponent<NetworkIdentity>(),weapon.GetDamage());
        }

        [Server]
        public void StartControl(IWeapon weapon)
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
            yield return new WaitForFixedUpdate();
            inProgress = false;
        }

        #endregion
    }
}