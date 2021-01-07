using BialskyShooter.ItemSystem.UI;

namespace BialskyShooter.EquipmentSystem.UI
{
    public interface IEquipmentSlotsContainer
    {
        void InjectItem(IItemSlot itemSlot);
    }
}
