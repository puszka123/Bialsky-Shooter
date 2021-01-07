using BialskyShooter.ItemSystem.UI;

namespace BialskyShooter.InventoryModule.UI
{
    public interface IInventorySlotsContainer
    {
        IItemSlot GetFirstAvailableSlot();
        void InjectItem(IItemSlot itemSlot);
    }
}
