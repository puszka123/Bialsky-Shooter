using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public abstract class ItemProperties : ScriptableObject
    {
        [SerializeField] protected Guid id = default;
        [SerializeField] protected GameObject prefab = default;
        [SerializeField] protected string uniqueName = default;
        [SerializeField] protected string iconPath = default;

        public Guid Id { get { return id; } }
        public string UniqueName { get { return uniqueName; } }
        public GameObject Prefab { get { return prefab; } }
        public string IconPath { get { return iconPath; } }
    }
}
