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
            ItemInformation itemInformation;

            public ItemSlotMock(ItemInformation itemInformation)
            {
                this.itemInformation = itemInformation;
            }

            public ItemInformation GetItemInformation()
            {
                return itemInformation;
            }

            public Guid GetItemId()
            {
                return itemInformation?.ItemId ?? Guid.Empty;
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

            public void Stack(Guid sourceItemId, int count)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public static bool SwapItems(IItemSlot source, IItemSlot destination)
        {
            if (source == null || destination == null) return false;
            var sourceSlotMock = new ItemSlotMock(source?.GetItemInformation());
            var destinationSlotMock = new ItemSlotMock(destination?.GetItemInformation());
            source.DragOutItem();
            destination.DragOutItem();
            if (sourceSlotMock.GetItemInformation() != null) destination.DragInItem(sourceSlotMock);
            if(destinationSlotMock.GetItemInformation() != null) source.DragInItem(destinationSlotMock);
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
