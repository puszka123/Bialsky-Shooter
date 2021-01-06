using BialskyShooter.ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem
{
    public interface IEquipmentSlotsContainer
    {
        void InjectItem(IItemSlot itemSlot);
    }
}
