using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Multiplayer.UI
{
    public class LobbyUpdater : NetworkBehaviour
    {
        [Inject] MyNetworkManager networkManager;

        private void Awake()
        {
            Player.playerInformationChanged += PlayerInformationChanged;
        }

        private void OnDestroy()
        {
            Player.playerInformationChanged -= PlayerInformationChanged;
            NetworkServer.UnSpawn(gameObject);
        }

        #region server

        [Command]
        public void CmdSetPlayerInformation(PlayerInformation playerInformation, NetworkIdentity identity)
        {
            var player = networkManager.PlayersList.First(p => p.GetComponent<NetworkIdentity>().connectionToClient == identity.connectionToClient);
            player.SetInformation(playerInformation);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            if (!hasAuthority) return;
            var playerInformation = FindObjectOfType<MenuController>().PlayerInformation;
            CmdSetPlayerInformation(playerInformation, GetComponent<NetworkIdentity>());
            FindObjectOfType<MenuController>().OnClientConnect();
            MenuController.clientOnGameStart += OnGameStart;
        }

        public override void OnStopClient()
        {
            if (!hasAuthority) return;
            MenuController.clientOnGameStart -= OnGameStart;
        }

        [Client]
        private void OnGameStart()
        {
            networkManager.StartGame();
        }

        [Client]
        void PlayerInformationChanged()
        {
            FindObjectOfType<MenuController>().UpdatePlayersPanel(networkManager.PlayersList.Where(p => p.PlayerInformation != null)
                .Select(p => p.PlayerInformation));
        }

        #endregion
    }
}