using BialskyShooter.InventoryModule.UI;
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

            public void DragInItem(IItemSlot itemSlot)
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

            public Guid DragOutItem()
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
            source.DragOutItem();
            destination.DragOutItem();
            if (sourceSlotMock.GetItemId() != Guid.Empty) destination.DragInItem(sourceSlotMock);
            if(destinationSlotMock.GetItemId() != Guid.Empty) source.DragInItem(destinationSlotMock);
            return true;
        }

        public static void SwapItems(IItemSlot source, IInventorySlotsContainer container)
        {
            var destination = container.GetSlot(source.GetItemId());
            if (destination == null) destination = container.GetFirstAvailableSlot();
            SwapItems(source, destination);
        }
    }
}
