using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.InventoryModule
{
    public class Inventory : NetworkBehaviour
    {
        SyncList<ItemInformation> syncItemInformations = new SyncList<ItemInformation>();

        public IList<ItemInformation> SyncItemInformations { get { return syncItemInformations; } }

        private IEnumerator Start()
        {
            //debug only
            if (NetworkServer.active && item1 != null)
            {
                yield return new WaitForSeconds(1f);
                item1 = Instantiate(item1);
                item2 = Instantiate(item2);
                item3 = Instantiate(item3);
                PickupItem(item1);
                PickupItem(item2);
                PickupItem(item3);
            }
        }

        #region Server

        [SerializeField] Item item1 = default;
        [SerializeField] Item item2 = default;
        [SerializeField] Item item3 = default;
        [SerializeField] IItem testItemProperties3;
        Dictionary<Guid, IItem> itemsDict;

        public override void OnStartServer()
        {
            itemsDict = new Dictionary<Guid, IItem>();
        }


        [Command]
        public void CmdLootItem(NetworkIdentity loot, Guid itemId)
        {
            LootItem(loot, itemId);
        }

        [Server]
        void LootItem(NetworkIdentity loot, Guid itemId)
        {
            Inventory lootInventory = loot.GetComponent<Inventory>();
            var item = lootInventory.ThrowAwayItem(itemId);
            if (item != null)
            {
                PickupItem(item);
            }
        }

        [Server]
        public IItem ThrowAwayItem(Guid itemId)
        {
            if (!itemsDict.ContainsKey(itemId)) return null;
            var item = itemsDict[itemId];
            itemsDict.Remove(itemId);
            var itemToRemove = syncItemInformations.Find(e => Guid.Parse(e.itemId) == itemId);
            syncItemInformations.Remove(itemToRemove);
            return item;
        }

        [Server]
        public void PickupItem(IItem item)
        {
            itemsDict[item.GetId()] = item;
            var itemInformation = new ItemInformation(item.GetId().ToString(),
                item.GetItem().IconPath,
                item.GetItem().UniqueName,
                item.GetItem().Stats.StatsList);
            syncItemInformations.Add(itemInformation);
            RpcPickupItem(itemInformation);
        }

        #endregion

        #region Client
        public event Action clientOnInventoryChanged;

        [Client]
        public void ClientLootItem(NetworkIdentity loot, Guid itemId)
        {
            if (itemId != Guid.Empty)
            {
                CmdLootItem(loot, itemId);
            }
        }

        [ClientRpc]
        void RpcPickupItem(ItemInformation itemInformation)
        {
            if (!hasAuthority) return;
            ClientPickupItem(itemInformation);
        }

        [Client]
        void ClientPickupItem(ItemInformation itemInformation)
        {
            clientOnInventoryChanged?.Invoke();
        }

        #endregion
    }
}
