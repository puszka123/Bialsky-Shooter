using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(Equipmentnstantiator))]
    public class WeaponUser : NetworkBehaviour
    {
        [Inject] Equipmentnstantiator equipmentInstantiator;
        IWeaponController weaponController;

        #region Server

        [Server]
        public override void OnStartServer()
        {
            equipmentInstantiator.serverOnItemInstantiated += EquipmentItemInstantiated;
            var weapon = equipmentInstantiator.GetInstantiatedItem(ItemSlotType.Weapon);
            if(weapon != null) weaponController = weapon.GetComponent<IWeaponController>();
        }

        public override void OnStopServer()
        {
            equipmentInstantiator.serverOnItemInstantiated -= EquipmentItemInstantiated;
        }

        [Server]
        private void EquipmentItemInstantiated(GameObject item, ItemSlotType itemSlotType)
        {
            if (itemSlotType == ItemSlotType.Weapon)
            {
                weaponController = item.GetComponent<IWeaponController>();
            }
        }

        [Server]
        public void UseWeapon(IWeapon weapon)
        {
            weaponController.StartControl(gameObject, weapon);
        }

        #endregion
    }
}
