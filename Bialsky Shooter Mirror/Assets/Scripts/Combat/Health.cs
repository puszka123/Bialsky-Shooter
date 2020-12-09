using BialskyShooter.ClassSystem;
using BialskyShooter.InventoryModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(CreatureStats))]
    public class Health : NetworkBehaviour
    {
        public event Action serverOnCreatureLose;
        public event Action clientOnCreatureLose;

        CreatureStats creatureStats;
        [SyncVar] float currentHealth;
        [SyncVar] bool isDefeated;

        public bool IsDefeated { get { return isDefeated; } }

        #region Server

        public override void OnStartServer()
        {
            creatureStats = GetComponent<CreatureStats>();
            currentHealth = creatureStats.GetStatValue(StatType.Health);
        }

        [Command]
        public void CmdTakeDamage(NetworkIdentity attacker, float damage)
        {
            TakeDamage(attacker, damage);
        }

        [Server]
        public void TakeDamage(NetworkIdentity attacker, float damage)
        {
            currentHealth -= damage;
            if(currentHealth <= 1f)
            {
                currentHealth = 1f;
                Lose();
                attacker
                    .GetComponent<Experience>()
                    .GainExperience(creatureStats.GetStatValue(StatType.ExperienceReward));
            }
        }

        [Server]
        void Lose()
        {
            isDefeated = true;
            serverOnCreatureLose?.Invoke();
            RpcOnCreatureLose();
        }

        #endregion

        #region Client

        [ClientRpc]
        void RpcOnCreatureLose()
        {
            clientOnCreatureLose?.Invoke();
        }

        #endregion
    }
}
