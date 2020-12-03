using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    public class Equipment : NetworkBehaviour
    {
        [SerializeField] Weapon weapon = null;

        public Weapon Weapon { get { return weapon; } }
    }
}
