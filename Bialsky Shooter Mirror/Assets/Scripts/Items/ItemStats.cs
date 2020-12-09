using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class ItemStats
    {
        [SerializeField] List<ItemStat> stats = default;

        public IEnumerable<ItemStat> Stats { get { return stats; } }
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
            foreach (var stat in stats)
            {
                statsBook[stat.statType] = stat;
            }
        }
    }
}
