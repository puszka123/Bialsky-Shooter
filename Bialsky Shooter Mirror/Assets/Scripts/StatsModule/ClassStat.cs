using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    [System.Serializable]
    public class ClassStat : Stat
    {
        public ClassStatType statType;


        public override T GetCopy<T>()
        {
            var res = new ClassStat
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
