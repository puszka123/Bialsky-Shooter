using BialskyShooter.Gameplay;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
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

        void AssignTeams()
        {
            foreach (var teamMember in FindObjectsOfType<TeamMember>())
            {
                if (teamMember.CompareTag("Player")) teamManager.AddToNewTeam(teamMember, Guid.NewGuid().ToString());
                else teamManager.AddToNeutral(teamMember);
            }
        }
    }
}
