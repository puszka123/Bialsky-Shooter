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
        #region Server
        [SerializeField] ItemProperties testItemProperties;
        Dictionary<Guid, Item> itemsDict;


        public override void OnStartServer()
        {
            itemsDict = new Dictionary<Guid, Item>();    
        }

        [ServerCallback]
        private IEnumerator Start()
        {
            //debug only
            if (testItemProperties != null)
            {
                yield return new WaitForSeconds(5f);
                var testItem = new Item(testItemProperties);
                PickupItem(testItem);
            }
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
            PickupItem(item);
        }

        [Server]
        Item ThrowAwayItem(Guid itemId)
        {
            var item = new Item(itemsDict[itemId]);
            itemsDict.Remove(itemId);
            RpcThrowAwayItem(itemId);
            return item;
        }

        [Server]
        void PickupItem(Item item)
        {
            itemsDict[item.Id] = item;
            RpcPickupItem(item.Id);
        }

        #endregion

        #region Client

        [SerializeField] List<string> itemsIdsStrings = new List<string>();
        List<Guid> itemsIds;

        public override void OnStartClient()
        {
            if (itemsIds == null) itemsIds = new List<Guid>();
        }

        [Client]
        public void ClientLootItem(NetworkIdentity loot, Guid itemId)
        {
            CmdLootItem(loot, itemId);
        }

        [Client]
        public Guid GetFirstItemId()
        {
            return itemsIds[0];
        }

        [ClientRpc]
        void RpcPickupItem(Guid itemId)
        {
            ClientPickupItem(itemId);
        }

        void ClientPickupItem(Guid itemId)
        {
            if(itemsIds == null) itemsIds = new List<Guid>();
            itemsIds.Add(itemId);
            itemsIdsStrings.Add(itemId.ToString());
        }

        [ClientRpc]
        void RpcThrowAwayItem(Guid itemId)
        {
            ClientThrowAwayItem(itemId);
        }

        [Client]
        void ClientThrowAwayItem(Guid itemId)
        {
            itemsIds.Remove(itemId);
            itemsIdsStrings.Remove(itemId.ToString());
        }

        #endregion
    }
}
