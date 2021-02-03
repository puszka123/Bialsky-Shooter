using BialskyShooter.Gameplay;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(TeamManager))]
    public class TeamAllocator : NetworkBehaviour, IRunnable
    {
        [SerializeField] BattleSceneManager.Priority priority = default;
        [Inject] TeamManager teamManager = null;

        public int Priority()
        {
            return (int)priority;
        }

        public void Run()
        {
            AssignTeams();
        }

        void Awake()
        {
            teamManager.Init();
        }

        //debug
        void AssignTeams()
        {
            var player = FindObjectsOfType<TeamMember>().First(e => e.CompareTag("Player"));
            teamManager.AddToNewTeam(player, Guid.NewGuid().ToString());
            foreach (var teamMember in FindObjectsOfType<TeamMember>())
            {
                if (teamMember.CompareTag("Player") || teamMember.name.Contains("Spawner")) continue;
                if (teamMember.name.StartsWith("Ally"))
                {
                    teamManager.AddToTeam(teamMember, player.TeamId);
                    teamMember.gameObject.GetComponentInChildren<Renderer>().material.color = Color.yellow;
                }
                else teamManager.AddToNeutral(teamMember);
            }
        }
    }
}
