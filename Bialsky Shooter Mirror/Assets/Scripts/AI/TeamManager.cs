using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class TeamManager : MonoBehaviour
    {
        public class Team
        {
            public static Guid NeutralId = Guid.NewGuid();
            public Guid Id { get; private set; }
            public string Name { get; set; }
            public Dictionary<Guid, TeamMember> MembersDictionary { get; private set; }
            public IList<TeamMember> Members { get; set; }

            public Team(string name)
            {
                MembersDictionary = new Dictionary<Guid, TeamMember>();
                Members = new List<TeamMember>();
                Name = name;
            }

            public Team(Guid id, string name) : this(name)
            {
                Id = id;
            }

            public void AddMember(TeamMember teamMember)
            {
                var newMemberId = Guid.NewGuid();
                MembersDictionary[newMemberId] = teamMember;
                teamMember.Init(newMemberId, Id);
                Members.Add(teamMember);
            }
        }
        public Dictionary<Guid, Team> TeamsDictionary { get; private set; }

        public void Init()
        {
            TeamsDictionary = new Dictionary<Guid, Team>();
        }

        public Guid AddToNewTeam(TeamMember teamMember, string teamName = "No name")
        {
            var newTeamId = Guid.NewGuid();
            var newTeam = new Team(newTeamId, teamName);
            newTeam.AddMember(teamMember);
            TeamsDictionary[newTeamId] = newTeam;
            return newTeamId;
        }

        public void AddToTeam(TeamMember teamMember, Guid teamId)
        {
            TeamsDictionary[teamId].AddMember(teamMember);
        }

        public void AddToNeutral(TeamMember teamMember)
        {
            if(!TeamsDictionary.ContainsKey(Team.NeutralId))
            {
                var neutralTeam = new Team(Team.NeutralId, "Neutral");
                TeamsDictionary[Team.NeutralId] = neutralTeam;
            }
            TeamsDictionary[Team.NeutralId].AddMember(teamMember);
        }

        public List<TeamMember> GetAllEnemies(Guid teamId)
        {
            var enemies = new List<TeamMember>();
            foreach (var pair in TeamsDictionary)
            {
                if (pair.Key == teamId) continue;
                enemies.AddRange(pair.Value.Members);
            }
            return enemies;
        }
    }
}