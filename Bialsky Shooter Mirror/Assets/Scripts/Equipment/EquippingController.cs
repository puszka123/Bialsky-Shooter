using BialskyShooter.InventoryModule;
using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Equipment))]
    public class EquippingController : NetworkBehaviour
    {
        Inventory inventory;
        Equipment equipment;
        EquipmentDisplay equipmentDisplay;

        #region Server

        public override void OnStartServer()
        {
            inventory = GetComponent<Inventory>();
            equipment = GetComponent<Equipment>();
            equipmentDisplay = FindObjectOfType<EquipmentDisplay>();
        }

        [Command]
        void CmdEquipItem(Guid itemId)
        {
            EquipItem(itemId);
        }

        [Server]
        void EquipItem(Guid itemId)
        {
            var item = inventory.ThrowAwayItem(itemId);
            var itemInformation = equipment.Equip(item);
            RpcEquipItem(itemInformation);
        }

        [Command]
        void CmdUnequipItem(Guid itemId)
        {
            UnequipItem(itemId);
        }

        [Server]
        void UnequipItem(Guid itemId)
        {
            var item = equipment.Unequip(itemId);
            inventory.PickupItem(item);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            InventoryItemSelection.clientOnItemSelected += ClientOnInventoryItemSelected;
            EquipmentItemSelection.clientOnItemSelected += ClientOnEquipmentItemSelected;
        }
        public override void OnStopAuthority()
        {
            InventoryItemSelection.clientOnItemSelected -= ClientOnInventoryItemSelected;
            EquipmentItemSelection.clientOnItemSelected -= ClientOnEquipmentItemSelected;

        }

        [Client]
        void ClientOnInventoryItemSelected(Guid itemId)
        {
            CmdEquipItem(itemId);
        }

        [Client]
        void ClientOnEquipmentItemSelected(Guid itemId)
        {
            CmdUnequipItem(itemId);
        }

        [ClientRpc]
        void RpcEquipItem(ItemInformation itemInformation)
        {
            ClientEquipItem(itemInformation);
        }

        void ClientEquipItem(ItemInformation itemInformation)
        {
            equipmentDisplay.DisplayItem(itemInformation);
        }

        #endregion
    }
}
