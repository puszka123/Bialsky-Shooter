using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;
using System;

namespace BialskyShooter.AI
{
    public class AllyBoxSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform selectionArea = null;
        TeamManager teamManager;
        TeamMember player;
        public IList<NetworkIdentity> SelectedAllies { get; private set; }

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
        }

        private void Update()
        {
            if (player == null) GetPlayer();
        }

        private void GetPlayer()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("PlayerCharacter"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    this.player = player.GetComponent<TeamMember>();
                    break;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            selectionArea.anchoredPosition = Mouse.current.position.ReadValue();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var difference = Mouse.current.position.ReadValue() - selectionArea.anchoredPosition;
            selectionArea.sizeDelta = difference;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ReselectAllies();
            selectionArea.sizeDelta = Vector2.zero;
        }

        private void ReselectAllies()
        {
            DeselectAllies();
            SelectAllies();
        }

        private void SelectAllies()
        {
            var allies = teamManager.GetAllAllies(player.TeamId);
            SelectedAllies = new List<NetworkIdentity>();
            foreach (var ally in allies)
            {
                if (ally.CompareTag("PlayerCharacter")) continue;
                var allyScreenPosition = Camera.main.WorldToScreenPoint(ally.transform.position);
                var min = selectionArea.anchoredPosition;
                var max = selectionArea.anchoredPosition + selectionArea.sizeDelta;
                if (allyScreenPosition.x >= min.x
                    && allyScreenPosition.x <= max.x
                    && allyScreenPosition.y >= min.y
                    && allyScreenPosition.y <= max.y)
                {
                    SelectedAllies.Add(ally.GetComponent<NetworkIdentity>());
                    ally.GetComponentInChildren<Renderer>().material.color = Color.black;
                }
            }
        }

        private void DeselectAllies()
        {
            if (SelectedAllies == null) return;
            foreach (var ally in SelectedAllies)
            {
                ally.GetComponentInChildren<Renderer>().material.color = Color.yellow;
            }
        }
    }
}
