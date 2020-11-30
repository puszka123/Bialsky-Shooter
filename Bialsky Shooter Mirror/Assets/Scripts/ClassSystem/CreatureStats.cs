using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    [RequireComponent(typeof(Experience))]
    public class CreatureStats : NetworkBehaviour
    {
        [SerializeField] Progression progression;
        [SerializeField] ClassType classType = default;
        [SyncVar] Stat health = null;
        [SyncVar] Stat power = null;
        [SyncVar] Stat stamina = null;
        [SyncVar] Stat agility = null;
        [SyncVar] Stat strength = null;

        public Stat Health { get { return health; } }
        public Stat Power { get { return power; } }
        public Stat Stamina { get { return stamina; } }
        public Stat Agility { get { return agility; } }
        public Stat Strength { get { return strength; } }

        public override void OnStartServer()
        {
            InitStats();
            UpdateStats(classType, GetComponent<Experience>().ExperiencePoints);
        }

        private void InitStats()
        {
            health = progression.GetStatDefinition(StatType.Health);
            power = progression.GetStatDefinition(StatType.Power);
            stamina = progression.GetStatDefinition(StatType.Stamina);
            agility = progression.GetStatDefinition(StatType.Agility);
            strength = progression.GetStatDefinition(StatType.Strength);
        }

        public void UpdateStats(ClassType classType, float experiencePoints)
        {
            UpdateStat(health, classType, experiencePoints);
            UpdateStat(power, classType, experiencePoints);
            UpdateStat(stamina, classType, experiencePoints);
            UpdateStat(agility, classType, experiencePoints);
            UpdateStat(strength, classType, experiencePoints);
        }

        public void UpdateStat(Stat stat, ClassType classType, float experiencePoints)
        {
            int level = GetLevel(classType, experiencePoints);
            stat.value = progression.GetStat(classType, stat.statType, level);
        }

        public int GetLevel(ClassType classType, float experiencePoints)
        {
            return progression.GetLevel(classType, experiencePoints);
        }
    }
}
