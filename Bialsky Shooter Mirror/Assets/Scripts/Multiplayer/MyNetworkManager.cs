using BialskyShooter.AI;
using BialskyShooter.ResourcesModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BialskyShooter.Multiplayer
{
    public class MyNetworkManager : NetworkManager
    {
        #region Server

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                var teamMember = player.GetComponent<TeamMember>();
                if (teamMember.TeamId == Guid.Empty)
                {
                    FindObjectOfType<TeamAllocator>().AssignToNewTeam(teamMember);
                    FindObjectOfType<PlayerSpawner>().SpawnPlayer(player.GetComponent<TeamMember>().TeamId, conn);
                    break;
                }
            }
        }

        #endregion

        #region Client


        #endregion
    }
}
