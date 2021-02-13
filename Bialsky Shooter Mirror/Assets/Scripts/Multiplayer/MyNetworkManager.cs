using BialskyShooter.ResourcesModule;
using Mirror;
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
            FindObjectOfType<PlayerSpawner>().SpawnPlayer(conn);
        }

        #endregion

        #region Client


        #endregion
    }
}
