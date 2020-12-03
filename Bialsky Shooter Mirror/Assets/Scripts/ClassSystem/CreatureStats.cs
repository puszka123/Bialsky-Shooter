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
        [SerializeField] Progression progression = null;
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

        Experience experience;


        public override void OnStartServer()
        {
            experience = GetComponent<Experience>();
            InitStats();
            UpdateStats();
        }

        private void InitStats()
        {
            health = progression.GetStatDefinition(StatType.Health);
            power = progression.GetStatDefinition(StatType.Power);
            stamina = progression.GetStatDefinition(StatType.Stamina);
            agility = progression.GetStatDefinition(StatType.Agility);
            strength = progression.GetStatDefinition(StatType.Strength);
        }

        public void UpdateStats()
        {
            UpdateStat(health);
            UpdateStat(power);
            UpdateStat(stamina);
            UpdateStat(agility);
            UpdateStat(strength);
        }

        public void UpdateStat(Stat stat)
        {
            stat.value = progression.GetStat(classType, stat.statType, GetLevel(experience.ExperiencePoints));
        }

        public int GetLevel(float experiencePoints)
        {
            return progression.GetLevel(classType, experiencePoints);
        }

        public float GetStatValue(StatType statType)
        {
            return progression.GetStat(classType, statType, GetLevel(experience.ExperiencePoints));
        }
    }
}
