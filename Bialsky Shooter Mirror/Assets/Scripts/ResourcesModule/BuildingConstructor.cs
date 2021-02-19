using BialskyShooter.AI;
using BialskyShooter.ResourcesModule.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingConstructor : NetworkBehaviour
    {
        [Inject] TeamMember teamMember;
        [Inject] TeamManager teamManager;
        [Inject] BuildingsStorage buildingsStorage;

        private void Awake()
        {
            BuildingSlot.clientOnBuildingPreviewEnd += OnBuildingPreviewEnd;
        }

        void OnDestroy()
        {
            BuildingSlot.clientOnBuildingPreviewEnd -= OnBuildingPreviewEnd;
        }

        #region server

        [Command]
        public void CmdConstructBuilding(Guid buildingId, Vector3 spawnPosition)
        {
            foreach (var building in buildingsStorage.AvailableBuildings)
            {
                if (building.Id == buildingId)
                {
                    ConstructBuilding(building, spawnPosition);
                    return;
                }
            }
        }

        [Server]
        void ConstructBuilding(Building building, Vector3 spawnPosition)
        {
            var buildingInstance = Instantiate(building.BuildingConfig.BuildingPrefab, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(buildingInstance, connectionToClient);
            buildingInstance.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            teamManager.GetComponent<TeamAllocator>().AssignToTeam(buildingInstance.GetComponent<TeamMember>(), teamMember.TeamId);
        }

        #endregion

        #region client

        [Client]
        void OnBuildingPreviewEnd(Guid buildingId, Vector3 spawnPosition)
        {
            if (!hasAuthority) return;
            CmdConstructBuilding(buildingId, spawnPosition);
        }

        #endregion
    }
}