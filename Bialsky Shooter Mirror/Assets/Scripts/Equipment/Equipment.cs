using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    public class Equipment : NetworkBehaviour
    {
        [SerializeField] WeaponSO weapon = default;
        [SerializeField] ShieldSO shield = default;
        [SerializeField] ChestSO chest = default;
        [SerializeField] HelmetSO helmet = default;
        [SerializeField] LegsSO legs = default;
        [SerializeField] BootsSO boots = default;

        public WeaponSO Weapon { get { return weapon; } }
        public ShieldSO Shield { get { return shield; } }
        public ChestSO Chest { get { return chest; } }
        public HelmetSO Helmet { get { return helmet; } }
        public LegsSO Legs { get { return legs; } }
        public BootsSO Boots { get { return boots; } }
    }
}
