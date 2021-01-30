using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] float spawnRange = 5f;
        [SerializeField] float spawnCreaturesCount = 5f; 
        CreatureFactoryBehaviour.CreatureFactory creatureFactory;
        MyNetworkManager networkManager;

        [ServerCallback]
        private IEnumerator Start()
        {

            yield return new WaitForSeconds(1f);
            SpawnEnemies();
        }

        [Inject]
        public void Construct(CreatureFactoryBehaviour.CreatureFactory creatureFactory, MyNetworkManager networkManager)
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            this.creatureFactory = creatureFactory;
            this.networkManager = networkManager;
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
