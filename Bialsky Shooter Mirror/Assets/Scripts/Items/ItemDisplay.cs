using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class ItemDisplay
    {
        Sprite icon;
        Guid itemId;

        public Guid ItemId { get { return itemId; } }
        public Sprite Icon { get { return icon; } }

        public ItemDisplay(Guid itemId, Sprite icon)
        {
            this.itemId = itemId;
            this.icon = icon;
        }
    }
}
