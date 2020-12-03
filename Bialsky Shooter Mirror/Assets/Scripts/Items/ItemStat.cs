using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class ItemStat
    {
        public string nameToDisplay;
        public float value;
        public bool display;
        public ItemStatType statType;
    }
}
