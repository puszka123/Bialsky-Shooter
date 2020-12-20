using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public abstract class WeaponController : NetworkBehaviour
    {
        #region Server

        public abstract void StartControl(IWeapon weapon);

        #endregion
    }
}
