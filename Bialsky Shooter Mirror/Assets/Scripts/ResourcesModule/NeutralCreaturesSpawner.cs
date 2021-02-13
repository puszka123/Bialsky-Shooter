using BialskyShooter.AI;
using BialskyShooter.Gameplay;
using BialskyShooter.Multiplayer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class NeutralCreaturesSpawner : Spawner
    {
        [Inject] protected TeamManager teamManager;
        [Inject] protected TeamMember teamMember;

        [SerializeField] protected float spawnRange = 5f;
        [SerializeField] protected float spawnCreaturesCount = 5f;

        TeamAllocator teamAllocator;
        public float SpawnRange { get { return spawnRange; } }

        [ServerCallback]
        private void Awake()
        {
            teamAllocator = teamManager.GetComponent<TeamAllocator>();
        }

        [Server]
        public override void Run()
        {
            teamAllocator.AssignToNeutral(teamMember);
            SpawnCreatures();
        }

        [Server]
        protected void SpawnCreatures(NetworkConnection conn = null)
        {
            for (int i = 0; i < spawnCreaturesCount; i++)
            {
                var creatureInstance = creatureFactory
                    .Create(GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
                NetworkServer.Spawn(creatureInstance);
                var color = teamAllocator.AssignToTeam(creatureInstance.GetComponent<TeamMember>(), teamMember.TeamId);
                creatureInstance.GetComponent<StateMachine>().Init(stateGraph);
                creatureInstance.GetComponent<Memory>().SetSpawn(this);
                creatureInstance.GetComponent<Memory>().SetColor(color);
            }
        }

        [Server]
        protected override Vector3 GetSpawnPosition()
        {
            var seed = UnityEngine.Random.insideUnitSphere * spawnRange;
            return new Vector3(transform.position.x + seed.x, transform.position.y, transform.position.z + seed.z);
        }
    }
}
