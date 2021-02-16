using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class Building : NetworkBehaviour
    {
        [SerializeField] BuildingConfig buildingConfig;
        public Guid Id { get; private set; }
        public BuildingConfig BuildingConfig { get { return buildingConfig; } }

        public void Init()
        {
            if (Id != Guid.Empty) return;
            Id = Guid.NewGuid();
        }
    }
}