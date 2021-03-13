using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class LeatherChest : Item, IChest
    {
        public LeatherChest(ItemSettings itemSettings) : base(itemSettings)
        {
        }

        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Chest;
        }
    }
}
