using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class ItemStats
    {
        [SerializeField] List<ItemStat> statsList = default;

        public IEnumerable<ItemStat> StatsList { get { return statsList; } }
        Dictionary<ItemStatType, ItemStat> statsBook;

        public ItemStat GetStat(ItemStatType statType)
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
            statsBook = new Dictionary<ItemStatType, ItemStat>();
            foreach (var stat in statsList)
            {
                statsBook[stat.statType] = stat;
            }
        }
    }
}
