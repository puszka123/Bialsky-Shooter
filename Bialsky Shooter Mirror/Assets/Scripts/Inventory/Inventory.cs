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
            PickupItem(item);
        }

        [Server]
        Item ThrowAwayItem(Guid itemId)
        {
            var item = new Item(itemsDict[itemId]);
            itemsDict.Remove(itemId);
            var itemToRemove = syncItemInformations.Find(e => Guid.Parse(e.itemId) == itemId);
            syncItemInformations.Remove(itemToRemove);
            return item;
        }

        [Server]
        void PickupItem(Item item)
        {
            itemsDict[item.Id] = item;
            syncItemInformations.Add(new ItemInformation(item.Id.ToString(), item.Properties.IconPath));
            RpcPickupItem(item.Id, item.Properties.IconPath);
        }

        #endregion

        #region Client

        [Client]
        public void ClientLootItem(NetworkIdentity loot, Guid itemId)
        {
            CmdLootItem(loot, itemId);
        }

        [ClientRpc]
        void RpcPickupItem(Guid itemId, string iconPath)
        {
            if (!hasAuthority) return;
            ClientPickupItem(itemId, iconPath);
        }

        void ClientPickupItem(Guid itemId, string iconPath)
        {
            Sprite icon = Resources.Load<Sprite>(iconPath);
            var displayItem = new ItemDisplay(itemId, icon);
            if (CompareTag("Player")) inventoryDisplay.DisplayItem(displayItem);
        }

        [Client]
        public IEnumerable<ItemDisplay> GetItemDisplays()
        {
            var itemDisplays = new List<ItemDisplay>();
            for (int i = 0; i < syncItemInformations.Count; i++)
            {
                var itemId = syncItemInformations[i].itemId;
                var icon = Resources.Load<Sprite>(syncItemInformations[i].iconPath);
                itemDisplays.Add(new ItemDisplay(Guid.Parse(itemId), icon));
            }

            return itemDisplays;
        }

        #endregion
    }
}
