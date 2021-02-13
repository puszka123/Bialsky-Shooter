using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using Mirror;
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

        [Inject]
        public void Construct(CreatureFactoryBehaviour.PlayerFactory creatureFactory)
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            this.creatureFactory = creatureFactory;

        }

        public override void Run()
        {
            
        }

        public void SpawnPlayer(NetworkConnection conn)
        {
            StartCoroutine(SpawnPlayerTimeout(conn));
        }

        IEnumerator SpawnPlayerTimeout(NetworkConnection conn)
        {
            yield return new WaitForSeconds(0.01f);
            SpawnCreatures(conn);
        }

        protected override Vector3 GetSpawnPosition()
        {
            return myNetworkManager.GetStartPosition().position;
        }

        protected override void SpawnCreatures(NetworkConnection conn = null)
        {
            var playerInstance = creatureFactory
                    .Create(GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
            NetworkServer.Spawn(playerInstance, conn);
            teamManager.GetComponent<TeamAllocator>().AssignToNewTeam(playerInstance.GetComponent<TeamMember>());
        }
    }
}
