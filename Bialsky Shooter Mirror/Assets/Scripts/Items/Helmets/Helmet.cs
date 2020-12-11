using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class Helmet : GenericItem<HelmetSO>
    {
        public Helmet(HelmetSO itemSO) : base(itemSO)
        {
        }

        public Helmet(GenericItem<HelmetSO> item) : base(item)
        {
        }

        public Helmet(Guid id, HelmetSO itemSO) : base(id, itemSO)
        {
        }
    }
}
