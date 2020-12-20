using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public interface IEquipmentItem : IItem
    {
        ItemSlotType GetItemSlotType();
    }
}
