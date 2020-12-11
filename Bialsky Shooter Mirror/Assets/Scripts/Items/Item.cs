using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Item : GenericItem<ItemSO>
    {
        public Item(ItemSO itemSO) : base(itemSO)
        {
        }

        public Item(GenericItem<ItemSO> item) : base(item)
        {
        }

        public Item(Guid id, ItemSO itemSO) : base(id, itemSO)
        {
        }
    }
}
