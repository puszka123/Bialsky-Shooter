using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem.UI
{
    [System.Serializable]
    public class ItemDisplay
    {
        Sprite icon;
        Guid itemId;
        List<ItemStat> itemStats;
        string itemName;

        public Guid ItemId { get { return itemId; } }
        public Sprite Icon { get { return icon; } }
        public IEnumerable<ItemStat> ItemStats { get { return itemStats; } }
        public string ItemName { get { return itemName; } }

        public ItemDisplay(ItemInformation itemInformation)
        {
            itemId = Guid.Parse(itemInformation.itemId);
            icon = Resources.Load<Sprite>(itemInformation.iconPath);
            itemStats = itemInformation.stats;
            itemName = itemInformation.itemName;
        }
    }
}
