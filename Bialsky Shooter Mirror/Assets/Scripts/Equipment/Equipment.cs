using BialskyShooter.ClassSystem;
using BialskyShooter.ItemSystem;
using BialskyShooter.StatsModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BialskyShooter.EquipmentSystem
{
    [RequireComponent(typeof(CreatureStats))]
    public class Equipment : NetworkBehaviour, IStatsModifier
    {
        [Inject] CreatureStats creatureStats;

        Dictionary<ItemSlotType, IEquipmentItem> equipmentItems;

        SyncList<ItemInformation> syncItemInformations = new SyncList<ItemInformation>();

        

        [ServerCallback]
        private void Awake()
        {
            InitEquipmentItems();
        }

        private void InitEquipmentItems()
        {
            equipmentItems = new Dictionary<ItemSlotType, IEquipmentItem>();
        }

        public IList<ItemInformation> ItemInformations { get { return syncItemInformations; } }

        public ItemInformation GetItemInformation(Guid itemId)
        {
            return syncItemInformations.FirstOrDefault(e => Guid.Parse(e.itemId) == itemId);
        }


        #region Server

        public event Action serverOnEquipmentChanged;

        [Server]
        public ItemInformation Equip(IEquipmentItem item)
        {
            equipmentItems[item.GetItemSlotType()] = item;
            var itemInformation = new ItemInformation(
                    item.GetId().ToString(),
                    item.GetItem().IconPath,
                    item.GetItem().UniqueName,
                    item.GetItemSlotType(),
                    item.GetItem().ItemStatsBook.StatsList);
            syncItemInformations.Add(itemInformation);
            serverOnEquipmentChanged?.Invoke();
            RpcEquipmentChanged();
            creatureStats.UpdateStats();
            return itemInformation;
        }

        [Server]
        public IEquipmentItem Unequip(Guid itemId)
        {
            foreach (var equipmentItem in equipmentItems.Values)
            {
                if (equipmentItem?.GetId() == itemId)
                {
                    RemoveFromSyncItems(itemId);
                    equipmentItems[equipmentItem.GetItemSlotType()] = null;
                    creatureStats.UpdateStats();
                    return equipmentItem;
                }
            }
            return null;
        }

        [Server]
        public IEquipmentItem Unequip(ItemSlotType itemSlotType)
        {
            var item = equipmentItems[itemSlotType];
            if (item != null && item.GetId() != Guid.Empty)
            {
                RemoveFromSyncItems(item.GetId());
                equipmentItems[itemSlotType] = null;
                creatureStats.UpdateStats();
            }
            return item;
        }

        [Server]
        public IEnumerable<IItem> UnequipAll()
        {
            var unequippedItems = new List<IItem>();
            foreach (var key in equipmentItems.Keys.ToList())
            {
                var item = Unequip(key);
                if (item != null && item.GetId() != Guid.Empty) unequippedItems.Add(item);
            }
            return unequippedItems;
        }

        [Server]
        void RemoveFromSyncItems(Guid itemId)
        {
            var itemToRemove = syncItemInformations.Find(e => Guid.Parse(e.itemId) == itemId);
            syncItemInformations.Remove(itemToRemove);
            serverOnEquipmentChanged?.Invoke();
            RpcEquipmentChanged();
        }

        [Server]
        public float GetStatAdditiveModifier(StatType statType)
        {
            float statTotalValue = 0f;
            if (equipmentItems == null) InitEquipmentItems();
            foreach (var item in equipmentItems.Where(e => e.Value != null).Select(e => e.Value.GetItem()))
            {
                var stats = item.ItemStatsBook.StatsList;
                foreach (var stat in stats)
                {
                    if (stat.type != statType) continue;
                    statTotalValue += stat.value;
                }
            }
            return statTotalValue;
        }

        [Server]
        public float GetStatPercentageModifier(StatType statType)
        {
            float statTotalValue = 0f;
            if (equipmentItems == null) InitEquipmentItems();
            foreach (var item in equipmentItems.Where(e => e.Value != null).Select(e => e.Value.GetItem()))
            {
                var stats = item.ItemStatsBook.StatsList;
                foreach (var stat in stats)
                {
                    if (stat.type != statType) continue;
                    statTotalValue += stat.percentageValue;
                }
            }
            return statTotalValue;
        }

        [Server]
        public T GetItem<T>(ItemSlotType itemSlotType)
        {
            if (!equipmentItems.ContainsKey(itemSlotType)) return default;
            return (T)equipmentItems[itemSlotType];
        }

        [Server]
        public bool Exists(Guid itemId)
        {
            return ItemInformations.FirstOrDefault(e => e.itemId == itemId.ToString()) != null;
        }

        #endregion

        #region Client

        public event Action clientOnEquipmentChanged;

        [ClientRpc]
        public void RpcEquipmentChanged()
        {
            clientOnEquipmentChanged?.Invoke();
        }

        #endregion
    }
}
