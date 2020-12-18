using BialskyShooter.InventoryModule;
using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.EquipmentSystem
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Equipment))]
    public class EquippingController : NetworkBehaviour
    {
        [Inject] Inventory inventory;
        [Inject] Equipment equipment;

        EquipmentDisplay equipmentDisplay;

        private void Start()
        {
            equipmentDisplay = FindObjectOfType<EquipmentDisplay>();
        }

        #region Server

        [Command]
        void CmdEquipItem(Guid itemId)
        {
            EquipItem(itemId);
        }

        [Server]
        void EquipItem(Guid itemId)
        {
            var item = inventory.ThrowAwayItem(itemId);
            if (item != null)
            {
                var itemInformation = equipment.Equip(item);
                RpcEquipItem(itemInformation);
            }
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
            EquipmentItemSelection.clientOnItemDraggedIn += ClientOnItemDraggedIn;
        }
        public override void OnStopAuthority()
        {
            InventoryItemSelection.clientOnItemSelected -= ClientOnInventoryItemSelected;
            EquipmentItemSelection.clientOnItemSelected -= ClientOnEquipmentItemSelected;
            EquipmentItemSelection.clientOnItemDraggedIn -= ClientOnItemDraggedIn;

        }

        [Client]
        private void ClientOnItemDraggedIn(Guid itemId)
        {
            CmdEquipItem(itemId);
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
            if (!hasAuthority) return;
            ClientEquipItem(itemInformation);
        }

        [Client]
        void ClientEquipItem(ItemInformation itemInformation)
        {
            equipmentDisplay.DisplayItem(itemInformation);
        }

        #endregion
    }
}
