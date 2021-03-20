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
        [SerializeField] GameObject lobbyUpdaterPrefab;

        public void Host()
        {
            singleton.StartHost();
        }

        #region Server
        public Queue<Player> Players { get; private set; } = new Queue<Player>();
        public List<Player> PlayersList { get; } = new List<Player>();

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            Players.Enqueue(conn.identity.GetComponent<Player>());
            PlayersList.Add(conn.identity.GetComponent<Player>());
            var instance = Instantiate(lobbyUpdaterPrefab);
            NetworkServer.Spawn(instance, conn);

        }

        public void StartGame()
        {
            ServerChangeScene("Map_Battle_01");
        }

        #endregion

        #region Client
        public static event Action clientOnClientConnect;

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            clientOnClientConnect?.Invoke();
        }

        #endregion
    }
}
