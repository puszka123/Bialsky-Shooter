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
        public List<ItemStat> stats;

        public ItemInformation() { }

        public ItemInformation(string id, string iconPath, string itemName, IEnumerable<ItemStat> stats)
        {
            itemId = id;
            this.iconPath = iconPath;
            this.itemName = itemName;
            this.stats = new List<ItemStat>(stats);
        }

        public ItemInformation(string id, string iconPath, string itemName, ItemSlotType slotType, IEnumerable<ItemStat> stats)
        {
            itemId = id;
            this.iconPath = iconPath;
            this.itemName = itemName;
            this.slotType = slotType;
            this.stats = new List<ItemStat>(stats);
        }

        public ItemInformation(Item item, ItemSlotType slotType)
        {
            itemId = item.Id.ToString();
            this.iconPath = item.ItemSO.IconPath;
            this.itemName = item.ItemSO.UniqueName;
            this.slotType = slotType;
            this.stats = new List<ItemStat>(item.ItemSO.Stats.StatsList);
        }
    }
}
