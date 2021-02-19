using BialskyShooter.AI;
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
        [Inject] TeamManager teamManager;

        private void Awake()
        {
            NetworkServer.Spawn(gameObject);
        }

        public override void Run()
        {
            
        }

        public void SpawnPlayer(Guid teamId, NetworkConnection conn)
        {
            StartCoroutine(SpawnPlayerTimeout(teamId, conn));
        }

        IEnumerator SpawnPlayerTimeout(Guid teamId, NetworkConnection conn)
        {
            yield return new WaitForSeconds(0.01f);
            SpawnCreatures(teamId, conn);
        }

        protected override Vector3 GetSpawnPosition()
        {
            return myNetworkManager.GetStartPosition().position;
        }

        protected void SpawnCreatures(Guid teamId, NetworkConnection conn = null)
        {
            if (teamId == Guid.Empty) throw new Exception("TeamId is null!");
            var playerInstance = creatureFactory
                    .Create(spawningCreaturePrefab, GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
            NetworkServer.Spawn(playerInstance, conn);
            playerInstance.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            teamManager.GetComponent<TeamAllocator>().AssignToTeam(playerInstance.GetComponent<TeamMember>(), teamId);

        }
    }
}
