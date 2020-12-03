using BialskyShooter.ClassSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(CreatureStats))]
    public class Health : NetworkBehaviour
    {
        CreatureStats creatureStats;
        [SyncVar] float currentHealth;

        #region Server

        public override void OnStartServer()
        {
            creatureStats = GetComponent<CreatureStats>();
            currentHealth = creatureStats.GetStatValue(StatType.Health);
        }

        [Command]
        public void CmdTakeDamage(float damage)
        {
            TakeDamage(damage);
        }

        [Server]
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            print(currentHealth);
        }

        [Server]
        void Lose()
        {
            throw new System.NotImplementedException("Losing is not implemented!");
        }

        #endregion
    }
}
