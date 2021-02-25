using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.StatsModule
{
    [System.Serializable]
    public abstract class Stat
    {
        public string nameToDisplay;
        public float value;
        public bool display;

        public abstract T GetCopy<T>() where T: Stat, new();
    }
}
