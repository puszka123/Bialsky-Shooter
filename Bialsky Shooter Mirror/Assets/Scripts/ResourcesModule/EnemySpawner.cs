using BialskyShooter.AI;
using BialskyShooter.Gameplay;
using BialskyShooter.Multiplayer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class EnemySpawner : NetworkBehaviour, IRunnable
    {
        [SerializeField] protected float spawnRange = 5f;
        [SerializeField] protected float spawnCreaturesCount = 5f;
        [SerializeField] protected BattleSceneManager.Priority priority = default;
        protected CreatureFactoryBehaviour.CreatureFactory creatureFactory;
        protected MyNetworkManager networkManager;

        public int Priority()
        {
            return (int)priority;
        }

        public void Run()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < spawnCreaturesCount; i++)
            {
                var enemyInstance = creatureFactory
                    .Create(GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
                NetworkServer.Spawn(enemyInstance);
                enemyInstance.GetComponent<Patrol>().SpawnerPosition = transform.position;
                enemyInstance.GetComponent<Patrol>().SpawnRange = spawnRange;
            }
        }

        private Vector3 GetSpawnPosition()
        {
            var seed = UnityEngine.Random.insideUnitSphere * spawnRange;
            return new Vector3(transform.position.x + seed.x, transform.position.y, transform.position.z + seed.z);
        }
    }
}
