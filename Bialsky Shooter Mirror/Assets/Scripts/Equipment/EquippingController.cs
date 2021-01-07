using BialskyShooter.InventoryModule;
using BialskyShooter.ItemSystem;
using BialskyShooter.ItemSystem.UI;
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
        [Inject] Inventory inventory = null;
        [Inject] Equipment equipment = null;

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
            if (item != null && item is IEquipmentItem)
            {
                var equipmentItem = (IEquipmentItem)item;
                UnequipItem(equipmentItem.GetItemSlotType());
                var itemInformation = equipment.Equip(equipmentItem);
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
            if (item != null) inventory.PickupItem(item);
        }

        [Server]
        void UnequipItem(ItemSlotType itemSlotType)
        {
            var item = equipment.Unequip(itemSlotType);
            if(item != null) inventory.PickupItem(item);
        }

        #endregion

        #region Client

        public override void OnStartAuthority()
        {
            InventoryItemSlot.clientOnItemSelected += ClientOnInventoryItemSelected;
            EquipmentItemSlot.clientOnItemSelected += ClientOnEquipmentItemSelected;
            EquipmentItemSlot.clientOnItemDraggedIn += ClientOnItemDraggedIn;
            EquipmentItemSlot.clientOnItemCleared += ClientOnEquipmentItemCleared;
        }
        public override void OnStopAuthority()
        {
            InventoryItemSlot.clientOnItemSelected -= ClientOnInventoryItemSelected;
            EquipmentItemSlot.clientOnItemSelected -= ClientOnEquipmentItemSelected;
            EquipmentItemSlot.clientOnItemDraggedIn -= ClientOnItemDraggedIn;
            EquipmentItemSlot.clientOnItemCleared -= ClientOnEquipmentItemCleared;

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

        [Client]
        void ClientOnEquipmentItemCleared(Guid itemId)
        {
            CmdUnequipItem(itemId);
        }

        [Client]
        public void ClientEquipItem(Guid itemId)
        {
            CmdEquipItem(itemId);
        }

        #endregion
    }
}
