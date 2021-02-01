using BialskyShooter.ItemSystem.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.InventoryModule.UI
{
    public class InventoryDisplayContainer : MonoBehaviour, IInventorySlotsContainer
    {
        [SerializeField] InventoryDisplay inventoryDisplay = null;

        public IItemSlot GetFirstAvailableSlot()
        {
            return inventoryDisplay.GetFirstAvailableSlot();
        }

        public IItemSlot GetSlot(Guid itemId)
        {
            return inventoryDisplay.GetSlot(itemId);
        }

        public void InjectItem(IItemSlot itemSlot)
        {
            inventoryDisplay.InjectItem(itemSlot);
        }
    }
}
