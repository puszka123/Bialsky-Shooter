using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Item
    {
        Guid id;
        ItemSO properties;
        public ItemSO Properties { get { return properties; } }
        public Guid Id { get { return id; } }

        public Item(ItemSO properties)
        {
            this.properties = properties;
            id = Guid.NewGuid();
        }

        public Item(Guid id, ItemSO properties)
        {
            this.properties = properties;
            this.id = id;
        }

        public Item(Item item)
        {
            properties = item.properties;
            id = item.id;
        }
    }
}
