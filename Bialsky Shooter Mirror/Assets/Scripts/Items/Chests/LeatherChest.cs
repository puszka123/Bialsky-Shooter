using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Leather Chest", menuName = "ScriptableObjects/Items/Leather Chest")]
    public class LeatherChest : Item, IChest
    {
        public ItemSlotType GetItemSlotType()
        {
            return ItemSlotType.Chest;
        }
    }
}
