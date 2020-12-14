using BialskyShooter.ItemSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    public class Equipment : NetworkBehaviour
    {
        [SerializeField] Weapon weapon = default;
        [SerializeField] Shield shield = default;
        [SerializeField] Chest chest = default;
        [SerializeField] Helmet helmet = default;
        [SerializeField] Legs legs = default;
        [SerializeField] Boots boots = default;

        public Weapon Weapon { get { return weapon; } }
        public Shield Shield { get { return shield; } }
        public Chest Chest { get { return chest; } }
        public Helmet Helmet { get { return helmet; } }
        public Legs Legs { get { return legs; } }
        public Boots Boots { get { return boots; } }

        SyncList<ItemInformation> syncItemInformations = new SyncList<ItemInformation>();

        public IEnumerable<ItemInformation> ItemInformations { get { return syncItemInformations; } }

        public ItemInformation GetItemInformation(Guid itemId)
        {
            return syncItemInformations.FirstOrDefault(e => Guid.Parse(e.itemId) == itemId);
        }


        #region Server

        public ItemInformation Equip(Item item)
        {
            ItemSlotType slotType = default;
            if (item.ItemSO is WeaponSO) 
            { 
                weapon = new Weapon(item.Id, item.ItemSO as WeaponSO);
                slotType = ItemSlotType.Weapon;
            }
            if (item.ItemSO is ShieldSO)
            {
                shield = new Shield(item.Id, item.ItemSO as ShieldSO);
                slotType = ItemSlotType.Shield;
            }
            if (item.ItemSO is HelmetSO)
            {
                helmet = new Helmet(item.Id, item.ItemSO as HelmetSO);
                slotType = ItemSlotType.Helmet;
            }
            if (item.ItemSO is ChestSO)
            {
                chest = new Chest(item.Id, item.ItemSO as ChestSO);
                slotType = ItemSlotType.Chest;
            }
            if (item.ItemSO is LegsSO)
            {
                legs = new Legs(item.Id, item.ItemSO as LegsSO);
                slotType = ItemSlotType.Legs;
            }
            if (item.ItemSO is BootsSO)
            {
                boots = new Boots(item.Id, item.ItemSO as BootsSO);
                slotType = ItemSlotType.Boots;
            }
            var itemInformation = new ItemInformation(
                    item.Id.ToString(),
                    item.ItemSO.IconPath,
                    item.ItemSO.UniqueName,
                    slotType,
                    item.ItemSO.Stats.StatsList);
            syncItemInformations.Add(itemInformation);
            return itemInformation;
        }

        public Item Unequip(Guid itemId)
        {
            var itemToRemove = syncItemInformations.Find(e => Guid.Parse(e.itemId) == itemId);
            syncItemInformations.Remove(itemToRemove);
            if (weapon != null && weapon.Id == itemId)
            {
                var item = new Item(weapon.Id, weapon.ItemSO);
                weapon = null;
                return item;
            }
            if (shield != null && shield.Id == itemId)
            {
                var item = new Item(shield.Id, shield.ItemSO);
                shield = null;
                return item;
            }
            if (helmet != null && helmet.Id == itemId)
            {
                var item = new Item(helmet.Id, helmet.ItemSO);
                helmet = null;
                return item;
            }
            if (chest != null && chest.Id == itemId)
            {
                var item = new Item(chest.Id, chest.ItemSO);
                chest = null;
                return item;
            }
            if (legs != null && legs.Id == itemId)
            {
                var item = new Item(legs.Id, legs.ItemSO);
                legs = null;
                return item;
            }
            if (boots != null && boots.Id == itemId)
            {
                var item = new Item(boots.Id, boots.ItemSO);
                boots = null;
                return item;
            }

            throw new Exception($"Invalid itemId ({itemId}) in public Item Unequip(Guid itemId) method.");
        }

        #endregion
    }
}
