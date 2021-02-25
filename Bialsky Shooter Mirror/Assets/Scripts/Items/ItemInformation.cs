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

        public ItemInformation(IItem item, ItemSlotType slotType)
        {
            itemId = item.GetId().ToString();
            this.iconPath = item.GetItem().IconPath;
            this.itemName = item.GetItem().UniqueName;
            this.slotType = slotType;
            this.stats = new List<Stat>(item.GetItem().ItemStatsBook.StatsList);
        }
    }
}
