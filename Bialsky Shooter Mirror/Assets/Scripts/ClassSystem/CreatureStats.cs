using BialskyShooter.BuffsModule;
using BialskyShooter.StatsModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ClassSystem
{
    [RequireComponent(typeof(Experience))]
    [RequireComponent(typeof(BuffsReceiver))]
    public class CreatureStats : NetworkBehaviour
    {
        [Inject] Experience experience = null;
        [Inject] BuffsReceiver buffsReceiver = null;


        public event Action<int> serverOnLevelUp;

        [SerializeField] Progression progression = null;
        [SerializeField] ClassType classType = default;
        [SerializeField] CreatureType creatureType = default;
        [SyncVar] Stat health = null;
        [SyncVar] Stat power = null;
        [SyncVar] Stat stamina = null;
        [SyncVar] Stat agility = null;
        [SyncVar] Stat strength = null;
        [SyncVar] int level = 1;

        public Stat Health { get { return health; } }
        public Stat Power { get { return power; } }
        public Stat Stamina { get { return stamina; } }
        public Stat Agility { get { return agility; } }
        public Stat Strength { get { return strength; } }
        public ClassType ClassType { get { return classType; } }
        public CreatureType CreatureType { get { return creatureType; } }

        public int Level { get { return level; } }


        #region Server

        public override void OnStartServer()
        {
            experience.serverOnExperienceGained += OnExperienceGained;
            buffsReceiver.serverBuffChanged += BuffChanged;
            InitStats();
            UpdateStats();
        }

        public override void OnStopServer()
        {
            experience.serverOnExperienceGained -= OnExperienceGained;
            buffsReceiver.serverBuffChanged -= BuffChanged;
        }

        [Server]
        private void InitStats()
        {
            health = progression.GetStatDefinition(StatType.Health);
            power = progression.GetStatDefinition(StatType.Power);
            stamina = progression.GetStatDefinition(StatType.Stamina);
            agility = progression.GetStatDefinition(StatType.Agility);
            strength = progression.GetStatDefinition(StatType.Strength);
            level = GetLevel();
        }

        [Server]
        void OnExperienceGained(float experience)
        {
            int lvl = GetLevel();
            if(lvl > level)
            {
                level = lvl;
                serverOnLevelUp?.Invoke(level);
                UpdateStats();
            }
        }

        [Server]
        private void BuffChanged(Buff buff, bool added)
        {
            UpdateStats();
        }

        [Server]
        public void UpdateStats()
        {
            UpdateStat(health);
            UpdateStat(power);
            UpdateStat(stamina);
            UpdateStat(agility);
            UpdateStat(strength);
            ForceStatsUpdate();
        }

        [Server]
        public void UpdateStat(Stat stat)
        {
            stat.value = GetStatValue(stat.type);
        }

        [Server]
        private void ForceStatsUpdate()
        {
            health = health.GetCopy();
            power = power.GetCopy();
            stamina = stamina.GetCopy();
            agility = agility.GetCopy();
            strength = strength.GetCopy();
        }

        [Server]
        public int GetLevel()
        {
            return progression.GetLevel(classType, experience.ExperiencePoints);
        }

        [Server]
        public float GetStatValue(StatType statType)
        {
            return progression.GetStat(classType, creatureType, statType, level) + GetTotalStatModifier(statType);
        }

        [Server]
        float GetTotalStatModifier(StatType statType)
        {
            float totalStatModifier = 0f;
            foreach (var statsModifier in GetComponents<IStatsModifier>())
            {
                totalStatModifier += statsModifier.GetStatModifier(statType);
            }
            return totalStatModifier;
        }

        #endregion
    }
}
