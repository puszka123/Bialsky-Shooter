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
        [Inject] TeamChecker teamChecker;
        public event Action serverOnCreatureLose;
        public event Action clientOnCreatureLose;
        [SyncVar] float currentHealth;
        [SyncVar] float maxHealth;
        [SyncVar] bool isDefeated;

        public bool IsDefeated { get { return isDefeated; } }
        public float CurrentHealth { get { return currentHealth; } }
        public float MaxHealth { get { return maxHealth; } }

        #region Server

        public override void OnStartServer()
        {
            currentHealth = creatureStats.GetStatValue(StatType.Health);
            maxHealth = creatureStats.GetStatValue(StatType.Health);
            creatureStats.serverOnLevelUp += OnLevelUp;
        }

        public override void OnStopServer()
        {
            creatureStats.serverOnLevelUp -= OnLevelUp;
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

        [Server]
        void OnLevelUp(int level)
        {
            maxHealth = creatureStats.GetStatValue(StatType.Health);
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
