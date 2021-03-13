using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    public interface IStatsModifier
    {
        float GetStatAdditiveModifier(StatType statType);
        float GetStatPercentageModifier(StatType statType);
    }
}
