using BialskyShooter.AI;
using BialskyShooter.BarriersModule;
using BialskyShooter.BuffsModule;
using BialskyShooter.ClassSystem;
using BialskyShooter.StatsModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(CreatureStats))]
    [RequireComponent(typeof(BuffsReceiver))]
    public class Health : NetworkBehaviour
    {
        [Inject] CreatureStats creatureStats = null;
        [Inject] TeamChecker teamChecker = null;
        [Inject] BuffsReceiver buffsReceiver = null;
        [Inject] BarrierHandler barrierHandler = null;

        public event System.Action serverOnCreatureLose;
        public event System.Action clientOnCreatureLose;
        [SyncVar] float currentHealth;
        [SyncVar] bool isDefeated;

        public bool IsDefeated { get { return isDefeated; } }
        public float CurrentHealth { get { return currentHealth; } }
        public float MaxHealth { get { return creatureStats.Health.value; } }

        private void Awake()
        {
            creatureStats = GetComponent<CreatureStats>();
        }

        #region Server

        public override void OnStartServer()
        {
            currentHealth = creatureStats.GetStatValue(StatType.Health);
            buffsReceiver.serverBuffChanged += BuffChanged;
        }

        public override void OnStopServer()
        {
            buffsReceiver.serverBuffChanged -= BuffChanged;
        }

        [Command]
        public void CmdTakeDamage(NetworkIdentity attacker, float damage)
        {
            TakeDamage(attacker, damage);
        }

        private void BuffChanged(Buff buff, bool added)
        {
            var currentHealthModifier = 0f;
            foreach (var buffStat in buff.buffStats)
            {
                if(buffStat.type == StatType.Health)
                {
                    currentHealthModifier = added ? buffStat.value : -buffStat.value;
                }
            }
            currentHealth += currentHealthModifier;
        }

        [Server]
        public void TakeDamage(NetworkIdentity attacker, float damage)
        {
            if (teamChecker.GetTeamType(attacker.GetComponent<TeamMember>().TeamId) == TeamType.Enemy
                && !attacker.GetComponent<Health>().IsDefeated)
            {
                damage = barrierHandler.Absorb(damage, MaxHealth, BarrierType.Health);
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
