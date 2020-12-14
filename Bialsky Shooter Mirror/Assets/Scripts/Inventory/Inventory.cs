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
        InventoryDisplay inventoryDisplay;
        SyncList<ItemInformation> syncItemInformations = new SyncList<ItemInformation>();

        private IEnumerator Start()
        {
            if (CompareTag("Player") && hasAuthority)
            {
                inventoryDisplay = FindObjectOfType<InventoryDisplay>();
                foreach (var item in GetItemDisplays())
                {
                    inventoryDisplay.DisplayItem(item);
                }
            }
            //debug only
            if (NetworkServer.active && testItemProperties != null)
            {
                yield return new WaitForSeconds(1f);
                var testItem = new Item(testItemProperties);
                var testItem2 = new Item(testItemProperties2);
                PickupItem(testItem);
                PickupItem(testItem2);
            }
        }

        #region Server

        [SerializeField] ItemSO testItemProperties = default;
        [SerializeField] ItemSO testItemProperties2 = default;
        Dictionary<Guid, Item> itemsDict;

        public override void OnStartServer()
        {
            itemsDict = new Dictionary<Guid, Item>();
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
        public Item ThrowAwayItem(Guid itemId)
        {
            if (!itemsDict.ContainsKey(itemId)) return null;
            var item = new Item(itemsDict[itemId]);
            itemsDict.Remove(itemId);
            var itemToRemove = syncItemInformations.Find(e => Guid.Parse(e.itemId) == itemId);
            syncItemInformations.Remove(itemToRemove);
            return item;
        }

        [Server]
        public void PickupItem(Item item)
        {
            itemsDict[item.Id] = item;
            var itemInformation = new ItemInformation(item.Id.ToString(),
                item.ItemSO.IconPath,
                item.ItemSO.UniqueName,
                item.ItemSO.Stats.StatsList);
            syncItemInformations.Add(itemInformation);
            RpcPickupItem(itemInformation);
        }

        #endregion

        #region Client

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
            var displayItem = new ItemDisplay(itemInformation);
            if (CompareTag("Player")) inventoryDisplay.DisplayItem(displayItem);
        }

        [Client]
        public IEnumerable<ItemDisplay> GetItemDisplays()
        {
            var itemDisplays = new List<ItemDisplay>();
            foreach (var item in syncItemInformations)
            {
                itemDisplays.Add(new ItemDisplay(item));
            }
            return itemDisplays;
        }

        #endregion
    }
}
