using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    public class StatsModifierProvider : NetworkBehaviour
    {
        [Server]
        public float GetTotalStatAdditiveModifier(StatType statType)
        {
            return GetTotalStatModifier(statType);
        }

        [Server]
        public float GetTotalStatPercentageModifier(StatType statType)
        {
            return GetTotalStatModifier(statType, true);
        }

        [Server]
        float GetTotalStatModifier(StatType statType, bool percentage = false)
        {
            float totalStatModifier = 0f;
            foreach (var statsModifier in GetComponents<IStatsModifier>())
            {
                totalStatModifier += percentage ? statsModifier.GetStatPercentageModifier(statType)
                                                : statsModifier.GetStatAdditiveModifier(statType);
            }
            return percentage ? totalStatModifier / 100 : totalStatModifier;
        }
    }
}
