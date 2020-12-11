using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Chest : GenericItem<ChestSO>
    {
        public Chest(ChestSO itemSO) : base(itemSO)
        {
        }

        public Chest(GenericItem<ChestSO> item) : base(item)
        {
        }

        public Chest(Guid id, ChestSO itemSO) : base(id, itemSO)
        {
        }
    }
}
