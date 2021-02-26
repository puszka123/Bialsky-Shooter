using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    [System.Serializable]
    public class Stat
    {
        public string nameToDisplay;
        public float value;
        public float percentageValue;
        public bool display;
        public StatType type;

        public Stat GetCopy()
        {
            return new Stat
            {
                nameToDisplay = nameToDisplay,
                value = value,
                percentageValue = percentageValue,
                display = display,
                type = type,
            };
        }
    }
}
