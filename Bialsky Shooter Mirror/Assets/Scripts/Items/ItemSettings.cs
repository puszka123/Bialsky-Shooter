using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Item Settings", menuName = "ScriptableObjects/Items/Item Settings")]
    public class ItemSettings : ScriptableObject, IItem
    {
        [SerializeField] protected Guid id = default;
        [SerializeField] protected GameObject prefab = default;
        [SerializeField] protected string uniqueName = default;
        [SerializeField] protected string iconPath = default;
        [SerializeField] protected ItemStats stats;

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }
        public GameObject Prefab { get { return prefab; } }
        public string IconPath { get { return iconPath; } }
        public ItemStats Stats { get { return stats; } }

        private void OnEnable()
        {
            id = Guid.NewGuid();
        }

        public Guid GetId()
        {
            return Id;
        }

        public ItemSettings GetItemSettings()
        {
            return this;
        }
    }
}
