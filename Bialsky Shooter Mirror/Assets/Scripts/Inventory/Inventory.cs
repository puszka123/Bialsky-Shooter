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
                yield return new WaitForSeconds(1f);
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
            RpcPickupItem(item.Id, item.Properties.IconPath);
        }

        #endregion

        #region Client

        [SerializeField] List<string> itemsIdsStrings = new List<string>();
        List<ItemDisplay> itemDisplays;

        public IEnumerable<ItemDisplay> ItemDisplays { get { return itemDisplays; } }

        public override void OnStartClient()
        {
            if (itemDisplays == null) itemDisplays = new List<ItemDisplay>();
        }

        [Client]
        public void ClientLootItem(NetworkIdentity loot, Guid itemId)
        {
            CmdLootItem(loot, itemId);
        }

        [Client]
        public Guid GetFirstItemId()
        {
            return itemDisplays[0].ItemId;
        }

        [ClientRpc]
        void RpcPickupItem(Guid itemId, string iconPath)
        {
            ClientPickupItem(itemId, iconPath);
        }

        void ClientPickupItem(Guid itemId, string iconPath)
        {
            if(itemDisplays == null) itemDisplays = new List<ItemDisplay>();
            Sprite icon = Resources.Load<Sprite>(iconPath);
            itemDisplays.Add(new ItemDisplay(itemId, icon));
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
            itemDisplays.Remove(itemDisplays.Find(e => e.ItemId == itemId));
            itemsIdsStrings.Remove(itemId.ToString());
        }

        #endregion
    }
}
