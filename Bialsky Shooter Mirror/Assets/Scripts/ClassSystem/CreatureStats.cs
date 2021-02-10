using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ClassSystem
{
    [RequireComponent(typeof(Experience))]
    public class CreatureStats : NetworkBehaviour
    {
        public event Action<int> serverOnLevelUp;

        [SerializeField] Progression progression = null;
        [SerializeField] ClassType classType = default;
        [SyncVar] ClassStat health = null;
        [SyncVar] ClassStat power = null;
        [SyncVar] ClassStat stamina = null;
        [SyncVar] ClassStat agility = null;
        [SyncVar] ClassStat strength = null;
        [SyncVar] int level = 1;


        public ClassStat Health { get { return health; } }
        public ClassStat Power { get { return power; } }
        public ClassStat Stamina { get { return stamina; } }
        public ClassStat Agility { get { return agility; } }
        public ClassStat Strength { get { return strength; } }
        public ClassType ClassType { get { return classType; } }

        public int Level { get { return level; } }

        [Inject] Experience experience = null;

        #region Server

        public override void OnStartServer()
        {
            experience.serverOnExperienceGained += OnExperienceGained;
            InitStats();
            UpdateStats();
        }

        public override void OnStopServer()
        {
            experience.serverOnExperienceGained -= OnExperienceGained;
        }

        [Server]
        private void InitStats()
        {
            health = progression.GetStatDefinition(ClassStatType.Health);
            power = progression.GetStatDefinition(ClassStatType.Power);
            stamina = progression.GetStatDefinition(ClassStatType.Stamina);
            agility = progression.GetStatDefinition(ClassStatType.Agility);
            strength = progression.GetStatDefinition(ClassStatType.Strength);
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
            }
        }

        [Server]
        public void UpdateStats()
        {
            UpdateStat(health);
            UpdateStat(power);
            UpdateStat(stamina);
            UpdateStat(agility);
            UpdateStat(strength);
        }

        [Server]
        public void UpdateStat(ClassStat stat)
        {
            stat.value = GetStatValue(stat.statType);
        }

        [Server]
        public int GetLevel()
        {
            return progression.GetLevel(classType, experience.ExperiencePoints);
        }

        [Server]
        public float GetStatValue(ClassStatType statType)
        {
            return progression.GetStat(classType, statType, level) + GetTotalStatModifier(statType);
        }

        [Server]
        float GetTotalStatModifier(ClassStatType statType)
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
