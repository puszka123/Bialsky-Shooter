using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public interface IItemSlot
    {
        ItemInformation GetItemInformation();
        Guid GetItemId();
        bool ReadyOnly();

        void DragInItem(IItemSlot itemSlot);
        Guid DragOutItem();
        void Stack(Guid sourceItemId, int count);
        void SetItemVisibility(bool visibility);
    }
}