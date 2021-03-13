using BialskyShooter.StatsModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.BuffsModule
{
    public class BuffsReceiver : NetworkBehaviour, IStatsModifier
    {
        public event Action<Buff, bool> serverBuffChanged;
        List<Buff> activeBuffs = new List<Buff>();
        Dictionary<Guid, Buff> buffsBook = new Dictionary<Guid, Buff>();

        public IEnumerable<Buff> ActiveBuffs { get { return activeBuffs; } } 

        [Server]
        public float GetStatAdditiveModifier(StatType statType)
        {
            return GetStatModifier(statType);
        }

        [Server]
        public float GetStatPercentageModifier(StatType statType)
        {
            return GetStatModifier(statType, true);
        }

        [Server]
        public float GetStatModifier(StatType statType, bool percentage = false)
        {
            var modifier = 0f;
            foreach (var stat in activeBuffs.SelectMany(b => b.buffStats))
            {
                if (stat.type == statType)
                {
                    modifier += percentage ? stat.percentageValue : stat.value;
                }
            }
            return modifier;
        }

        [Server]
        public void AddBuff(Buff buff)
        {
            if (buffsBook.ContainsKey(buff.id))
            {
                buffsBook[buff.id].duration = buff.duration;
            }
            else
            {
                buffsBook[buff.id] = buff;
                activeBuffs.Add(buff);
                serverBuffChanged?.Invoke(buff, true);
            }
        }

        [Server]
        public bool RemoveBuff(Buff buff)
        {
            var success = activeBuffs.Remove(buff);
            if (success)
            {
                buffsBook.Remove(buff.id);
                serverBuffChanged?.Invoke(buff, false);
            }
            return success;
        }

    }
}