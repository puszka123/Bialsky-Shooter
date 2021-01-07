using BialskyShooter.ItemSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.InventoryModule.UI
{
    public class InventoryDisplayContainer : MonoBehaviour, IInventorySlotsContainer
    {
        [SerializeField] InventoryDisplay inventoryDisplay;

        public IItemSlot GetFirstAvailableSlot()
        {
            return inventoryDisplay.GetFirstAvailableSlot();
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            inventoryDisplay.InjectItem(itemSlot);
        }
    }
}
