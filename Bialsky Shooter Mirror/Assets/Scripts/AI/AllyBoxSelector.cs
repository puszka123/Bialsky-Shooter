using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Mirror;
using System;
using BialskyShooter.ResourcesModule;

namespace BialskyShooter.AI
{
    public class AllyBoxSelector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform selectionArea = null;
        AllySelector allySelector;

        [ClientCallback]
        private void Update()
        {
            if (allySelector == null) GetAllySelector();
        }

        [Client]
        private void GetAllySelector()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    allySelector = player.GetComponent<AllySelector>();
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
            allySelector.CmdReselectAllies(selectedTeamMembers());
            selectionArea.sizeDelta = Vector2.zero;
        }

        List<NetworkIdentity> selectedTeamMembers()
        {
            var selectedAlliesPositions = new List<NetworkIdentity>();
            foreach (var ally in FindObjectsOfType<TeamMember>())
            {
                if (ally.CompareTag("PlayerCharacter") || ally.GetComponent<Spawner>() != null) continue;
                var allyScreenPosition = Camera.main.WorldToScreenPoint(ally.transform.position);
                var min = selectionArea.anchoredPosition;
                var max = selectionArea.anchoredPosition + selectionArea.sizeDelta;
                if (allyScreenPosition.x >= min.x
                    && allyScreenPosition.x <= max.x
                    && allyScreenPosition.y >= min.y
                    && allyScreenPosition.y <= max.y)
                {
                    selectedAlliesPositions.Add(ally.GetComponent<NetworkIdentity>());
                    ally.GetComponentInChildren<Renderer>().material.color = Color.black;
                }
            }
            return selectedAlliesPositions;
        }
    }
}
