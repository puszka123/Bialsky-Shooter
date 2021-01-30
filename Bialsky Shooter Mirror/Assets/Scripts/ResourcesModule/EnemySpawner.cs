using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] protected float spawnRange = 5f;
        [SerializeField] protected float spawnCreaturesCount = 5f; 
        protected CreatureFactoryBehaviour.CreatureFactory creatureFactory;
        protected MyNetworkManager networkManager;

        [ServerCallback]
        private IEnumerator Start()
        {

            yield return new WaitForSeconds(1f);
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
