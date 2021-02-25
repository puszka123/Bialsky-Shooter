using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    [System.Serializable]
    public class ItemStat : Stat
    {
        public ItemStatType statType;

        public override T GetCopy<T>()
        {
            var res = new ItemStat
            {
                nameToDisplay = nameToDisplay,
                value = value,
                display = display,
                statType = statType,
            };
            return res as T;
        }
    }
}
