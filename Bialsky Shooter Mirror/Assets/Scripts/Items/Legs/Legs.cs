using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Legs : GenericItem<LegsSO>
    {
        public Legs(LegsSO itemSO) : base(itemSO)
        {
        }

        public Legs(GenericItem<LegsSO> item) : base(item)
        {
        }

        public Legs(Guid id, LegsSO itemSO) : base(id, itemSO)
        {
        }
    }
}
