using BialskyShooter.ItemSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public class WeaponUser : NetworkBehaviour
    {
        WeaponController weaponController;

        #region Server

        public override void OnStartServer()
        {
            weaponController = gameObject.GetComponentInChildren<WeaponController>();
        }

        [Server]
        public void UseWeapon(IWeapon weapon)
        {
            weaponController.StartControl(gameObject, weapon);
        }

        #endregion
    }
}
