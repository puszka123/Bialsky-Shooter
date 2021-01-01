using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] ItemSettings itemSettings = default;
        [SerializeField] ItemSlotType itemSlotType = default;
        Guid id;

        private void Awake()
        {
            id = Guid.NewGuid();
        }

        public Guid GetId()
        {
            return id;
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