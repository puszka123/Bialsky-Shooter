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
            
        }

        void Awake()
        {
            teamManager.Init();
        }

        public void AssignToNeutral(TeamMember teamMember)
        {
            teamManager.AddToNeutral(teamMember);
        }

        public float AssignToTeam(TeamMember teamMember, Guid teamId)
        {
            float value = teamManager.TeamsDictionary.Keys.ToList().IndexOf(teamId) / teamManager.TeamsDictionary.Keys.ToList().Count;
            teamManager.AddToTeam(teamMember, teamId);
            return value;
        }

        public float AssignToNewTeam(TeamMember teamMember)
        {
            var teamId = teamManager.AddToNewTeam(teamMember, teamMember.name);
            float value = teamManager.TeamsDictionary.Keys.ToList().IndexOf(teamId) / teamManager.TeamsDictionary.Keys.ToList().Count;
            return value;
        }
    }
}
