using BialskyShooter.Multiplayer;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class EnemySpawner : NetworkBehaviour
    {
        CreatureFactoryBehaviour.CreatureFactory creatureFactory;
        MyNetworkManager networkManager;

        [ServerCallback]
        private IEnumerator Start()
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            yield return new WaitForSeconds(1f);
            SpawnEnemies();
        }

        [Inject]
        public void Construct(CreatureFactoryBehaviour.CreatureFactory creatureFactory, MyNetworkManager networkManager)
        {
            this.creatureFactory = creatureFactory;
            this.networkManager = networkManager;
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < 1; i++)
            {
                var enemyInstance = creatureFactory
                    .Create(networkManager.GetStartPosition().position, Quaternion.identity)
                    .gameObject;
                NetworkServer.Spawn(enemyInstance);
            }
        }
    }
}
