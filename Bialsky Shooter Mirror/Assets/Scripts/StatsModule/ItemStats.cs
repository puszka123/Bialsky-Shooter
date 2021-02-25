using BialskyShooter.ClassSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    [System.Serializable]
    public class ItemStats
    {
        [SerializeField] List<Stat> statsList = default;

        public IEnumerable<Stat> StatsList { get { return statsList; } }

        Dictionary<StatType, Stat> statsBook;

        public Stat GetStat(StatType statType)
        {
            if (statsBook == null) InitStatsBook();
            if(statsBook.ContainsKey(statType))
            {
                return statsBook[statType];
            }
            else
            {
                return null;
            }
        }

        private void InitStatsBook()
        {
            statsBook = new Dictionary<StatType, Stat>();
            foreach (var stat in statsList)
            {
                statsBook[stat.type] = stat;
            }
        }
    }
}
