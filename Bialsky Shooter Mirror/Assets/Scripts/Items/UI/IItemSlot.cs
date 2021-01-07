using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public interface IItemSlot
    {
        Guid GetItemId();
        Sprite GetItemIcon();
        bool ReadyOnly();

        void InjectItem(IItemSlot itemSlot);
        Guid ClearItem();
        void SetItemVisibility(bool visibility);
    }
}