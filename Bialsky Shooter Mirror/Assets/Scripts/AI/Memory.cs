using BialskyShooter.ResourcesModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class Memory : NetworkBehaviour
    {
        public Vector3 SpawnerPosition { get; set; }
        public float SpawnRange { get; set; }

        public void SetSpawn(EnemySpawner spawner)
        {
            SpawnerPosition = spawner.transform.position;
            SpawnRange = spawner.SpawnRange;
        }
    }
}
