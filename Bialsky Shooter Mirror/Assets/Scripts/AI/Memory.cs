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
        [SyncVar] float color = 1f;
        public Vector3 SpawnerPosition { get; set; }
        public float SpawnRange { get; set; }

        private void Start()
        {
            GetComponentInChildren<Renderer>().material.color = new Color(color, color, color);
        }

        public void SetSpawn(NeutralCreaturesSpawner spawner)
        {
            SpawnerPosition = spawner.transform.position;
            SpawnRange = spawner.SpawnRange;
        }

        public void SetColor(float color)
        {
            this.color = color;
            GetComponentInChildren<Renderer>().material.color = new Color(color, color, color);
        }
    }
}
