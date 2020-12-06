using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    public class Equipment : NetworkBehaviour
    {
        [SerializeField] WeaponProperties weapon = null;

        public WeaponProperties Weapon { get { return weapon; } }
    }
}
