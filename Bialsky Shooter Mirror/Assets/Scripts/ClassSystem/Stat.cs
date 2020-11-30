using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    [System.Serializable]
    public class Stat
    {
        public string nameToDisplay;
        [HideInInspector] public float value;
        public bool display;
        public StatType statType;
    }
}
