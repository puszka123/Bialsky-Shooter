using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class WeaponStats
    {
        public ItemStat damage;
        public ItemStat range;
        public ItemStat cooldown;
    }
}
