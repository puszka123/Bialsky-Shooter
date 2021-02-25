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
        public bool display;
        public StatType type;

        public Stat GetCopy()
        {
            return new Stat
            {
                nameToDisplay = nameToDisplay,
                value = value,
                display = display,
                type = type,
            };
        }
    }
}
