using BialskyShooter.StatsModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemSettings", menuName = "ScriptableObjects/Items/ItemSettings")]
    public class ItemSettings : ScriptableObject
    {
        [SerializeField] protected Guid id = default;
        [SerializeField] protected GameObject prefab = default;
        [SerializeField] protected string uniqueName = default;
        [SerializeField] protected Sprite icon = default;
        [SerializeField] protected ItemStats itemStatsBook;

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }
        public GameObject Prefab { get { return prefab; } }
        public Sprite Icon { get { return icon; } }
        public ItemStats ItemStatsBook { get { return itemStatsBook; } }

        private void OnEnable()
        {
            id = Guid.NewGuid();
        }
    }
}
