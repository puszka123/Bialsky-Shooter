using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class Item : IItem
    {
        public Guid Id { get; private set; }
        public ItemSettings ItemSettings { get; private set; }

        public Item(ItemSettings itemSettings)
        {
            Id = Guid.NewGuid();
            ItemSettings = itemSettings;
        }

        public Guid GetId()
        {
            return Id;
        }

        public ItemSettings GetItemSettings()
        {
            return ItemSettings;
        }
    }
}
