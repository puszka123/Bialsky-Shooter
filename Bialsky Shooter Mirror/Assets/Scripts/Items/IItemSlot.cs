using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem
{
    public interface IItemSlot
    {
        Guid GetItemId();
        Image GetItemImage();
        bool ReadyOnly();

        void InjectItem(IItemSlot itemSlot);
        Guid ClearItem();
    }
}