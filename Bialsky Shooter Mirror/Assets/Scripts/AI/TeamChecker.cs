using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(TeamMember))]
    public class TeamChecker : MonoBehaviour
    {
        [Inject] TeamMember teamMember = null;

        public TeamType GetTeamType(Guid teamId)
        {
            if (TeamManager.Team.NeutralId == teamId) return TeamType.Neutral;
            if (teamMember.TeamId == teamId) return TeamType.Own;
            return TeamType.Enemy;
        }
    }
}
