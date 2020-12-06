using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public abstract class WeaponProperties : ItemProperties
    {
        [SerializeField] protected WeaponStats stats = null;

        public WeaponStats Stats { get { return stats; } }
    }
}
