using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class ItemInformation
    {
        public string itemId;
        public string iconPath;

        public ItemInformation() { }

        public ItemInformation(string id, string iconPath)
        {
            itemId = id;
            this.iconPath = iconPath;
        }
    }
}
