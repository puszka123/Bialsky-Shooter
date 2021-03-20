using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Multiplayer
{
    public class Player : NetworkBehaviour
    {
        [Inject] MyNetworkManager networkManager;
        public static event Action playerInformationChanged;

        [SyncVar(hook =nameof(OnPlayerNameChanged))] PlayerInformation playerInformation;
        public PlayerInformation PlayerInformation { get { return playerInformation; } }

        #region server

        public override void OnStartServer()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SetInformation(PlayerInformation playerInformation)
        {
            this.playerInformation = playerInformation;
        }

        #endregion

        #region client

        public override void OnStartClient()
        {
            if (NetworkServer.active) return;
            DontDestroyOnLoad(gameObject);
            networkManager.PlayersList.Add(this);
        }

        [Client]
        public void OnPlayerNameChanged(PlayerInformation oldInfo, PlayerInformation newInfo)
        {
            playerInformationChanged?.Invoke();
        }

        #endregion
    }
}
