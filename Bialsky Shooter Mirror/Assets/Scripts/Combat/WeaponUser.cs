using BialskyShooter.ClassSystem;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.ItemSystem;
using BialskyShooter.StatsModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(Equipmentnstantiator))]
    [RequireComponent(typeof(StatsModifierProvider))]
    public class WeaponUser : NetworkBehaviour
    {
        [Inject] Equipmentnstantiator equipmentInstantiator = null;
        [Inject] StatsModifierProvider statsModifierProvider = null;
        IWeaponController weaponController;
        Transform weaponTransform;
        public bool WeaponInUse { get; set; }
        public bool WeaponAsDefence { get; set; }

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
                weaponController.OnStartControl += OnStartControl;
                weaponController.OnStopControl += OnStopControl;
                weaponTransform = item.transform;
            }
        }

        [Server]
        private void OnStartControl(bool attack)
        {
            WeaponInUse = true;
            WeaponAsDefence = !attack;
        }

        [Server]
        private void OnStopControl()
        {
            WeaponInUse = false;
        }


        [Server]
        public void UseWeapon(IWeapon weapon, bool attack = true)
        {
            var buffs = new Dictionary<StatType, Vector2>
            {
                { 
                    StatType.Cooldown, 
                    new Vector2(
                            statsModifierProvider.GetTotalStatAdditiveModifier(StatType.Cooldown),
                            statsModifierProvider.GetTotalStatPercentageModifier(StatType.Cooldown))
                },
            };
            weaponController.StartControl(gameObject, weapon, buffs, attack);
        }

        [Server]
        public void ResetDefence()
        {
            weaponController.ResetDefenceTimer();
        }

        [Server]
        internal void Terminate()
        {
            weaponController.Terminate();
        }

        [Server]
        public Transform GetWeaponTransform()
        {
            return weaponTransform;
        }

        #endregion
    }
}
