using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class ItemInformation
    {
        public string itemId;
        public string itemName;
        public string iconPath;
        public ItemSlotType slotType;
        public List<Stat> stats;
        public int count;
        public bool stackable;

        public Guid ItemId { get { return Guid.Parse(itemId); } }

        public ItemInformation() { }

        public ItemInformation(string id, string iconPath, string itemName, IEnumerable<Stat> stats)
        {
            itemId = id;
            this.iconPath = iconPath;
            this.itemName = itemName;
            this.stats = new List<Stat>(stats);
        }

        public ItemInformation(string id, string iconPath, string itemName, ItemSlotType slotType, IEnumerable<Stat> stats)
        {
            itemId = id;
            this.iconPath = iconPath;
            this.itemName = itemName;
            this.slotType = slotType;
            this.stats = new List<Stat>(stats);
        }

        public ItemInformation(IItem item, ItemSlotType slotType) : this(item)
        {
            this.slotType = slotType;
        }

        public ItemInformation(IStackable stackable) : this((IItem)stackable)
        {
            count = stackable.GetCount();
            this.stackable = true;
        }

        public ItemInformation(IItem item)
        {
            itemId = item.GetId().ToString();
            iconPath = item.GetItem().IconPath;
            itemName = item.GetItem().UniqueName;
            stats = new List<Stat>(item.GetItem().ItemStatsBook.StatsList);
        }
    }
}
