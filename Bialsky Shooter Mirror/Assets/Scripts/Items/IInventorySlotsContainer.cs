using BialskyShooter.ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.InventoryModule
{
    public interface IInventorySlotsContainer
    {
        IItemSlot GetFirstAvailableSlot();
    }
}
