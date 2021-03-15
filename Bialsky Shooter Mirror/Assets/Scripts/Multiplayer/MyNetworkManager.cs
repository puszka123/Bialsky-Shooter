using BialskyShooter.AI;
using BialskyShooter.ResourcesModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BialskyShooter.Multiplayer
{
    public class MyNetworkManager : NetworkManager
    {
        #region Server
        public Queue<NetworkConnection> NetworkConnections { get; private set; } = new Queue<NetworkConnection>();

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            var player = GameObject.FindGameObjectsWithTag("Player")
                .First(p => p.GetComponent<NetworkIdentity>().connectionToClient == conn);
            NetworkConnections.Enqueue(conn);
        }

        #endregion

        #region Client


        #endregion
    }
}
