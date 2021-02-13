using BialskyShooter.AI;
using Mirror;
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

        [SerializeField] GameObject swordmanBuildingPrefab = null;
       
        #region server

        [Command]
        public void CmdConstructBuilding()
        {
            var buildingInstance = Instantiate(swordmanBuildingPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(buildingInstance, connectionToClient);
            buildingInstance.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            teamManager.GetComponent<TeamAllocator>().AssignToTeam(buildingInstance.GetComponent<TeamMember>(), teamMember.TeamId);
        }

        #endregion

        #region client

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) return;
            if(Keyboard.current.leftCtrlKey.isPressed 
                && Keyboard.current.qKey.wasPressedThisFrame)
            {
                CmdConstructBuilding();
            }
        }

        #endregion
    }
}