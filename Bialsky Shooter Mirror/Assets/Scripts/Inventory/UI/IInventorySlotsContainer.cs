using BialskyShooter.ItemSystem.UI;
using System;

namespace BialskyShooter.InventoryModule.UI
{
    public interface IInventorySlotsContainer
    {
        IItemSlot GetFirstAvailableSlot();
        IItemSlot GetSlot(Guid itemId);
        void InjectItem(IItemSlot itemSlot);
    }
}
