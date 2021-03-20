using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;
using System;

namespace BialskyShooter.Multiplayer.UI
{
    public class MenuController : MonoBehaviour
    {
        public static event Action clientOnGameStart;

        [SerializeField] GameObject clientTypePanel;
        [SerializeField] GameObject playerInfoPanel;
        [SerializeField] GameObject initPlayerPanel;
        [SerializeField] GameObject lobbyPanel;
        [SerializeField] GameObject joinPanel;
        [SerializeField] Button playerInfoOkButton;
        [SerializeField] Button joinOkButton;
        [SerializeField] Button startGameButton;
        [SerializeField] GameObject playerPanelPrefab;
        [SerializeField] GameObject playersPanel;
        PlayerInformation playerInformation;
        public PlayerInformation PlayerInformation { get { return playerInformation; } }

        private void Awake()
        {
            playerInfoOkButton.interactable = false;
            playerInfoPanel.SetActive(true);
        }

        public void PlayerInfoPanelOk()
        {
            playerInfoPanel.SetActive(false);
            clientTypePanel.SetActive(true);
        }

        public void OnPlayerNameChanged(string playerName)
        {
            playerInformation = new PlayerInformation(playerName);
            playerInfoOkButton.interactable = !string.IsNullOrEmpty(playerName);
        }

        public void OnHostSelected()
        {
            clientTypePanel.SetActive(false);
            NetworkManager.singleton.StartHost();
        }

        public void OnJoinSelected()
        {
            joinOkButton.interactable = false;
            clientTypePanel.SetActive(false);
            joinPanel.SetActive(true);
        }

        public void JoinOkButton()
        {
            joinOkButton.interactable = false;
            NetworkManager.singleton.networkAddress = joinPanel.GetComponent<JoinPanel>().IPAddress;
            NetworkManager.singleton.StartClient();
        }
        public void OnClientConnect()
        {
            joinPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }

        public void OnIPAddressChanged(string ipAddress)
        {
            joinOkButton.interactable = !string.IsNullOrEmpty(ipAddress);
        }

        public void UpdatePlayersPanel(IEnumerable<PlayerInformation> playerInformations)
        {
            RedrawPlayersPanel(playerInformations);
        }

        private void RedrawPlayersPanel(IEnumerable<PlayerInformation> playerInformations)
        {
            DestroyAllPlayerPanels();
            AddPlayers(playerInformations);
        }

        private void DestroyAllPlayerPanels()
        {
            var children = new List<GameObject>();
            foreach (Transform child in playersPanel.transform)
            {
                if (child == playersPanel.transform) continue;
                children.Add(child.gameObject);
            }
            foreach (var child in children)
            {
                Destroy(child);
            }
        }

        public void AddPlayers(IEnumerable<PlayerInformation> playerInformations)
        {
            foreach (var item in playerInformations)
            {
                AddPlayer(item);
            }
        }

        public void AddPlayer(PlayerInformation playerInformation)
        {
            lobbyPanel.SetActive(true);
            var instance = Instantiate(playerPanelPrefab, playersPanel.transform);
            var playerPanel = instance.GetComponent<PlayerPanel>();
            playerPanel.SetName(playerInformation?.Name);
        }

        public void StartGame()
        {
            clientOnGameStart?.Invoke();
        }
    }
}
