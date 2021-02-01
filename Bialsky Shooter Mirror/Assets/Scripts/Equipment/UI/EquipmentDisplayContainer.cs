using BialskyShooter.ItemSystem.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EquipmentSystem.UI
{
    public class EquipmentDisplayContainer : MonoBehaviour, IEquipmentSlotsContainer
    {
        [SerializeField] EquipmentDisplay equipmentDisplay = null;
        public void InjectItem(IItemSlot itemSlot)
        {
            equipmentDisplay.InjectItem(itemSlot);
        }
    }
}
