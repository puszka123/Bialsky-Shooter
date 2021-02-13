using BialskyShooter.AI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingSpawner : Spawner
    {
        [Inject] TeamMember teamMember;
        [Inject] TeamAllocator teamAllocator;

        [SerializeField] protected GameObject spawn;
        public override void Run()
        {
            SpawnCreatures();
        }

        protected override Vector3 GetSpawnPosition()
        {
            return spawn.transform.position;
        }

        protected override void SpawnCreatures(NetworkConnection conn = null)
        {
            var creatureInstance = creatureFactory
                    .Create(GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
            NetworkServer.Spawn(creatureInstance);
            teamAllocator.AssignToTeam(creatureInstance.GetComponent<TeamMember>(), teamMember.TeamId);
        }
    }
}
