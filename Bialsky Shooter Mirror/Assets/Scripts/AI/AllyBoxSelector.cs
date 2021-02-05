using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;

namespace BialskyShooter.AI
{
    public class AllyBoxSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform selectionArea;
        [SerializeField] RectTransform canvas;
        TeamManager teamManager;
        TeamMember player;

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
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
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
            SelectAllies();
            selectionArea.sizeDelta = Vector2.zero;
        }

        private void SelectAllies()
        {
            var allies = teamManager.GetAllAllies(player.TeamId);
            foreach (var ally in allies)
            {
                var allyScreenPosition = Camera.main.WorldToScreenPoint(ally.transform.position);
                var min = selectionArea.anchoredPosition;
                var max = selectionArea.anchoredPosition + selectionArea.sizeDelta;
                if (allyScreenPosition.x >= min.x
                    && allyScreenPosition.x <= max.x
                    && allyScreenPosition.y >= min.y
                    && allyScreenPosition.y <= max.y)
                {
                    ally.GetComponentInChildren<Renderer>().material.color = Color.black;
                }
            }
        }
    }
}
