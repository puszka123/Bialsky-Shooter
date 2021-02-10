using BialskyShooter.AI;
using BialskyShooter.ClassSystem;
using BialskyShooter.InventoryModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(CreatureStats))]
    public class Health : NetworkBehaviour
    {
        [Inject] CreatureStats creatureStats = null;
        [Inject] TeamChecker teamChecker = null;
        public event System.Action serverOnCreatureLose;
        public event System.Action clientOnCreatureLose;
        [SyncVar] float currentHealth;
        [SyncVar] bool isDefeated;

        public bool IsDefeated { get { return isDefeated; } }
        public float CurrentHealth { get { return currentHealth; } }
        public float MaxHealth { get { return creatureStats.Health.value; } }

        #region Server

        public override void OnStartServer()
        {
            currentHealth = creatureStats.GetStatValue(ClassStatType.Health);
        }

        [Command]
        public void CmdTakeDamage(NetworkIdentity attacker, float damage)
        {
            TakeDamage(attacker, damage);
        }

        [Server]
        public void TakeDamage(NetworkIdentity attacker, float damage)
        {
            if (teamChecker.GetTeamType(attacker.GetComponent<TeamMember>().TeamId) == TeamType.Enemy
                && !attacker.GetComponent<Health>().IsDefeated)
            {
                currentHealth -= damage;
            }
            if (currentHealth <= 1f)
            {
                currentHealth = 1f;
                Lose();
                attacker
                    .GetComponent<Experience>()
                    .GainExperience(creatureStats.GetStatValue(ClassStatType.ExperienceReward));
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
