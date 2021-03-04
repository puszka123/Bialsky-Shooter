using BialskyShooter.StatsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.BuffsModule
{
    [System.Serializable]
    public class Buff
    {
        public List<BuffStat> buffStats;
        public float duration;

        public Buff Create()
        {
            return new Buff
            {
                buffStats = buffStats,
                duration = duration,
            };
        }
    }
}
