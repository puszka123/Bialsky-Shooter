using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Boots : GenericItem<BootsSO>
    {
        public Boots(BootsSO itemSO) : base(itemSO)
        {
        }

        public Boots(GenericItem<BootsSO> item) : base(item)
        {
        }

        public Boots(Guid id, BootsSO itemSO) : base(id, itemSO)
        {
        }
    }
}
