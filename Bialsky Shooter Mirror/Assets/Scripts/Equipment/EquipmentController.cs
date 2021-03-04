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
    public class EquipmentController : NetworkBehaviour
    {
        [Inject] Inventory inventory = null;
        [Inject] Equipment equipment = null;
        [SerializeField] Item weapon;

        #region Server

        [ServerCallback]
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            if (weapon != null)
            {
                EquipItem(Instantiate(weapon));
            }
        }

        [Command]
        void CmdEquipItem(Guid itemId)
        {
            EquipItem(itemId);
        }

        [Server]
        void EquipItem(Guid itemId)
        {
            if (equipment.Exists(itemId)) return;
            var item = inventory.ThrowAwayItem(itemId);
            if (item != null && item is IEquipmentItem)
            {
                var equipmentItem = (IEquipmentItem)item;
                UnequipItem(equipmentItem.GetItemSlotType());
                var itemInformation = equipment.Equip(equipmentItem);
                RpcEquipItem(itemInformation);
            }
        }

        [Server]
        void EquipItem(Item item)
        {
            if (item != null && item is IEquipmentItem)
            {
                var equipmentItem = (IEquipmentItem)item;
                var itemInformation = equipment.Equip(equipmentItem);
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
            if (item != null)
            {
                inventory.PickupItem(item);
                RpcUneuipItem(new ItemInformation(item, item.GetItemSlotType()));
            }
        }

        [Server]
        void UnequipItem(ItemSlotType itemSlotType)
        {
            var item = equipment.Unequip(itemSlotType);
            if (item != null)
            {
                inventory.PickupItem(item);
                RpcUneuipItem(new ItemInformation(item, item.GetItemSlotType()));
            }
        }

        [Command]
        void CmdUnequipAll()
        {
            ServerUnequipAll();
        }

        [Server]
        public void ServerUnequipAll()
        {
            var unequippedItems = equipment.UnequipAll();
            foreach (var item in unequippedItems)
            {
                inventory.PickupItem(item);
            }
        }

        #endregion

        #region Client
        public event Action<ItemInformation> clientOnItemEquipped;
        public event Action<ItemInformation> clientOnItemUnequipped;

        public override void OnStartAuthority()
        {
            InventoryItemSlot.clientOnItemSelected += ClientOnInventoryItemSelected;
            EquipmentItemSlot.clientOnItemSelected += ClientOnEquipmentItemSelected;
            EquipmentItemSlot.clientOnItemDraggedIn += ClientOnEquipmentItemDraggedIn;
            EquipmentItemSlot.clientOnItemDraggedOut += ClientOnEquipmentItemDraggedOut;
        }
        public override void OnStopAuthority()
        {
            InventoryItemSlot.clientOnItemSelected -= ClientOnInventoryItemSelected;
            EquipmentItemSlot.clientOnItemSelected -= ClientOnEquipmentItemSelected;
            EquipmentItemSlot.clientOnItemDraggedIn -= ClientOnEquipmentItemDraggedIn;
            EquipmentItemSlot.clientOnItemDraggedOut -= ClientOnEquipmentItemDraggedOut;

        }

        [Client]
        private void ClientOnEquipmentItemDraggedOut(Guid itemId)
        {
            CmdUnequipItem(itemId);
        }

        [Client]
        private void ClientOnEquipmentItemDraggedIn(Guid itemId)
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
        public void ClientEquipItem(Guid itemId)
        {
            CmdEquipItem(itemId);
        }

        [Client]
        public void ClientUnequipAll()
        {
            CmdUnequipAll();
        }


        [ClientRpc]
        public void RpcEquipItem(ItemInformation itemInformation)
        {
            clientOnItemEquipped?.Invoke(itemInformation);
        }

        [ClientRpc]
        public void RpcUneuipItem(ItemInformation itemInformation)
        {
            clientOnItemUnequipped?.Invoke(itemInformation);
        }

        #endregion
    }
}
