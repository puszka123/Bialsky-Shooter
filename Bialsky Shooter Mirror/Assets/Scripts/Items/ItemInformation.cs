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
        public string iconPath;
        public ItemSlotType slotType;
        public List<ItemStat> stats;

        public ItemInformation() { }

        public ItemInformation(string id, string iconPath)
        {
            itemId = id;
            this.iconPath = iconPath;
        }

        public ItemInformation(string id, string iconPath, ItemSlotType slotType, IEnumerable<ItemStat> stats)
        {
            itemId = id;
            this.iconPath = iconPath;
            this.slotType = slotType;
            this.stats = new List<ItemStat>(stats);
        }
    }
}
