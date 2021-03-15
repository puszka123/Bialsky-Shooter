using BialskyShooter.Multiplayer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class PlayerSpawner : Spawner
    {
        [Inject] MyNetworkManager myNetworkManager;

        private void Awake()
        {
            NetworkServer.Spawn(gameObject);
        }

        public override void Run()
        {
            SpawnPlayer(myNetworkManager.NetworkConnections.Dequeue());
        }

        protected override Vector3 GetSpawnPosition()
        {
            return myNetworkManager.GetStartPosition().position;
        }

        protected void SpawnPlayer(NetworkConnection conn = null)
        {
            var playerInstance = creatureFactory
                    .Create(spawningCreaturePrefab, GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
            NetworkServer.Spawn(playerInstance, conn);
            playerInstance.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);

        }
    }
}
