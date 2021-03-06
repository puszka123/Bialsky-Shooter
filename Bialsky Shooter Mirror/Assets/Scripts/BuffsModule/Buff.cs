using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.BuffsModule
{
    [System.Serializable]
    public class Buff
    {
        public Guid id;
        public List<BuffStat> buffStats;
        public float duration;

        public Buff()
        {
            id = Guid.NewGuid();
        }

        public Buff Create()
        {
            return new Buff
            {
                buffStats = buffStats,
                duration = duration,
                id = id,
            };
        }
    }
}
