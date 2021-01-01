using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public class Chest : MonoBehaviour, IChest
    {
        [SerializeField] ItemSettings itemSettings = default;
        [SerializeField] ItemSlotType itemSlotType = default;

        public Guid GetId()
        {
            return itemSettings.GetId();
        }

        public ItemSettings GetItemSettings()
        {
            return itemSettings;
        }

        public ItemSlotType GetItemSlotType()
        {
            return itemSlotType;
        }
    }
}
