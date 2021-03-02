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

        void DragInItem(IItemSlot itemSlot);
        Guid DragOutItem();
        void SetItemVisibility(bool visibility);
    }
}