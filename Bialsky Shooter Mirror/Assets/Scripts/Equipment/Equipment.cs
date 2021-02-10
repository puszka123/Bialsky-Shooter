using BialskyShooter.ClassSystem;
using BialskyShooter.ItemSystem;
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

        public IWeapon Weapon { get; private set; }
        public IShield Shield { get; private set; }
        public IChest Chest { get; private set; }
        public IHelmet Helmet { get; private set; }
        public ILegs Legs { get; private set; }
        public IBoots Boots { get; private set; }

        Dictionary<ItemSlotType, IEquipmentItem> equipmentItems;

        SyncList<ItemInformation> syncItemInformations = new SyncList<ItemInformation>();

        [SerializeField] Item weapon;

        [ServerCallback]
        private void Start()
        {
            weapon = Instantiate(weapon);
            Weapon = (IWeapon)weapon;
            InitEquipmentItems();
        }

        private void InitEquipmentItems()
        {
            equipmentItems = new Dictionary<ItemSlotType, IEquipmentItem>();
            equipmentItems[ItemSlotType.Boots] = Boots;
            equipmentItems[ItemSlotType.Chest] = Chest;
            equipmentItems[ItemSlotType.Helmet] = Helmet;
            equipmentItems[ItemSlotType.Weapon] = Weapon;
            equipmentItems[ItemSlotType.Legs] = Legs;
            equipmentItems[ItemSlotType.Shield] = Shield;
        }

        public IEnumerable<ItemInformation> ItemInformations { get { return syncItemInformations; } }

        public ItemInformation GetItemInformation(Guid itemId)
        {
            return syncItemInformations.FirstOrDefault(e => Guid.Parse(e.itemId) == itemId);
        }


        #region Server

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
            RpcEquipmentChanged();
            creatureStats.UpdateStats();
            return itemInformation;
        }

        [Server]
        public IItem Unequip(Guid itemId)
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
        public IItem Unequip(ItemSlotType itemSlotType)
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
            RpcEquipmentChanged();
        }

        [Server]
        public float GetStatModifier(ClassStatType statType)
        {
            float statTotalValue = 0f;
            if (equipmentItems == null) InitEquipmentItems();
            foreach (var item in equipmentItems.Where(e => e.Value != null).Select(e => e.Value.GetItem()))
            {
                var stats = item.ItemStatsBook.ClassStats;
                foreach (var stat in stats)
                {
                    if (stat.statType != statType) continue;
                    statTotalValue += stat.value;
                }
            }
            return statTotalValue;
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
