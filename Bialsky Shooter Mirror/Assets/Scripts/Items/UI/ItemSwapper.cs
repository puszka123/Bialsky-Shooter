using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public static class ItemSwapper
    {
        class ItemSlotMock : IItemSlot
        {
            Guid id;
            Sprite icon;

            public ItemSlotMock(Guid id, Sprite icon)
            {
                this.id = id;
                this.icon = icon;
            }

            public Sprite GetItemIcon()
            {
                return icon;
            }

            public Guid GetItemId()
            {
                return id;
            }

            #region unused

            public void InjectItem(IItemSlot itemSlot)
            {
                throw new NotImplementedException();
            }

            public bool ReadyOnly()
            {
                throw new NotImplementedException();
            }

            public void SetItemVisibility(bool visibility)
            {
                throw new NotImplementedException();
            }

            public Guid ClearItem()
            {
                throw new NotImplementedException();
            }
            #endregion
        }

        public static bool SwapItems(IItemSlot source, IItemSlot destination)
        {
            if (source == null || destination == null) return false;
            var sourceSlotMock = new ItemSlotMock(source?.GetItemId() ?? Guid.Empty,
                                                source?.GetItemIcon());
            var destinationSlotMock = new ItemSlotMock(destination?.GetItemId() ?? Guid.Empty,
                                                destination?.GetItemIcon());
            source.ClearItem();
            destination.ClearItem();
            if (sourceSlotMock.GetItemId() != Guid.Empty) destination.InjectItem(sourceSlotMock);
            if(destinationSlotMock.GetItemId() != Guid.Empty) source.InjectItem(destinationSlotMock);
            return true;
        }
    }
}
