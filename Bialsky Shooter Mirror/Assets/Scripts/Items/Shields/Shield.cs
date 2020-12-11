using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Shield : GenericItem<ShieldSO>
    {
        public Shield(ShieldSO itemSO) : base(itemSO)
        {
        }

        public Shield(GenericItem<ShieldSO> item) : base(item)
        {
        }

        public Shield(Guid id, ShieldSO itemSO) : base(id, itemSO)
        {
        }
    }
}
