using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public abstract class Item : ScriptableObject, IItem
    {
        [SerializeField] protected Guid id = default;
        [SerializeField] protected GameObject prefab = default;
        [SerializeField] protected string uniqueName = default;
        [SerializeField] protected string iconPath = default;
        [SerializeField] protected ItemStats itemStatsBook;

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }
        public GameObject Prefab { get { return prefab; } }
        public string IconPath { get { return iconPath; } }
        public ItemStats ItemStatsBook { get { return itemStatsBook; } }

        private void OnEnable()
        {
            id = Guid.NewGuid();
        }

        public Guid GetId()
        {
            return Id;
        }

        public Item GetItem()
        {
            return this;
        }
    }
}
