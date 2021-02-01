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
    public class TeamAllocator : NetworkBehaviour
    {
        [Inject] TeamManager teamManager = null;

        void Awake()
        {
            teamManager.Init();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(5f);
            foreach (var teamMember in FindObjectsOfType<TeamMember>())
            {
                if (teamMember.CompareTag("Player")) teamManager.AddToNewTeam(teamMember, Guid.NewGuid().ToString());
                else teamManager.AddToNeutral(teamMember);
            }
        }
    }
}
