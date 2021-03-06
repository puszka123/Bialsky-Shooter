using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public interface IWeaponController
    {
        Action<bool> OnStartControl { get; set; }
        Action OnStopControl { get; set; }
        void StartControl(GameObject user, IWeapon weapon, bool attack = true);
        void ResetDefenceTimer();
        void Terminate();
    }
}
