using BialskyShooter.AI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingSpawner : Spawner
    {
        [Inject] protected TeamMember teamMember;
        [Inject] protected TeamManager teamManager;

        [SerializeField] protected GameObject spawn;
        ResourcesStore resourcesStore;

        #region server

        [Server]
        public void Init(ResourcesStore resourcesStore)
        {
            this.resourcesStore = resourcesStore;
        }

        [Command]
        public void CmdSpawnCreature()
        {
            if (resourcesStore == null) throw new Exception($"{name}: Building Spawner not initialized!");
            var spawnRequirements = spawningCreaturePrefab.GetComponent<SpawnRequirements>();
            if (resourcesStore.Affordable(spawnRequirements.SpawnCost))
            {
                SpawnCreature();
                resourcesStore.ExtractResources(spawnRequirements.SpawnCost);
            }
        }

        [Server]
        public override void Run()
        {

        }

        [Server]
        protected override Vector3 GetSpawnPosition()
        {
            return spawn.transform.position;
        }

        [Server]
        protected void SpawnCreature(NetworkConnection conn = null)
        {
            if (teamMember.TeamId == Guid.Empty) throw new Exception("BuildingSpawner: teamMember.TeamId == Guid.Empty"); 
            var creatureInstance = creatureFactory
                    .Create(spawningCreaturePrefab, GetSpawnPosition(), Quaternion.identity)
                    .gameObject;
            NetworkServer.Spawn(creatureInstance);
            creatureInstance.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            var color = teamManager.GetComponent<TeamAllocator>().AssignToTeam(creatureInstance.GetComponent<TeamMember>(), teamMember.TeamId);
            creatureInstance.GetComponent<StateMachine>().Init(stateGraph);
            creatureInstance.GetComponent<Memory>().SetColor(color);
        }

        #endregion


        #region client

        [ClientCallback]
        void Update()
        {
            if (!hasAuthority) return;
            if (Keyboard.current.leftCtrlKey.isPressed
                && Keyboard.current.leftShiftKey.isPressed
                && Keyboard.current.rKey.wasPressedThisFrame)
            {
                CmdSpawnCreature();
            }
        }

        #endregion
    }
}
