using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public class WeaponUser : NetworkBehaviour
    {
        IWeaponController weaponController;

        #region Server

        public override void OnStartServer()
        {
            weaponController = gameObject.GetComponentInChildren<IWeaponController>();
        }

        [Server]
        public void UseWeapon(IWeapon weapon)
        {
            weaponController.StartControl(gameObject, weapon);
        }

        #endregion
    }
}
