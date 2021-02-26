using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public interface WeaponController
    {
        void StartControl(GameObject user, IWeapon weapon);
    }
}
