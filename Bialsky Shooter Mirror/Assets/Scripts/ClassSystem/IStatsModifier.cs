using BialskyShooter.StatsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    public interface IStatsModifier
    {
        float GetStatAdditiveModifier(StatType statType);
        float GetStatPercentageModifier(StatType statType);
    }
}
