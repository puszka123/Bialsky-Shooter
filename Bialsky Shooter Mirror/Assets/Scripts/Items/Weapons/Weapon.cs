using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Weapon : GenericItem<WeaponSO>
    {
        public Weapon(WeaponSO itemSO) : base(itemSO)
        {
        }

        public Weapon(GenericItem<WeaponSO> item) : base(item)
        {
        }

        public Weapon(Guid id, WeaponSO itemSO) : base(id, itemSO)
        {
        }
    }
}
