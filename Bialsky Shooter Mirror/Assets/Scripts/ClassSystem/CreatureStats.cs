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

        public int Level { get { return level; } }

        [Inject] Experience experience;

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
        public void UpdateStat(Stat stat)
        {
            stat.value = progression.GetStat(classType, stat.statType, GetLevel());
        }

        [Server]
        public int GetLevel()
        {
            return progression.GetLevel(classType, experience.ExperiencePoints);
        }

        [Server]
        public float GetStatValue(StatType statType)
        {
            return progression.GetStat(classType, statType, level);
        }

        #endregion
    }
}
