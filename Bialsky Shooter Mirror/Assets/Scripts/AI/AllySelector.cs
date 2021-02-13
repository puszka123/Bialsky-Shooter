using BialskyShooter.ResourcesModule;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    public class AllySelector : NetworkBehaviour
    {
        [Inject] TeamManager teamManager;
        public IList<NetworkIdentity> SelectedAllies { get; private set; }

        [Command]
        public void CmdReselectAllies(List<NetworkIdentity> selectedTeamMembers)
        {
            var idk = gameObject.GetInstanceID();
            DeselectAllies();
            SelectAllies(selectedTeamMembers);
        }

        [Server]
        private void SelectAllies(List<NetworkIdentity> selectedTeamMembers)
        {
            var allies = teamManager.GetAllAllies(GetComponent<TeamMember>().TeamId);
            SelectedAllies = new List<NetworkIdentity>();
            foreach (var teamMember in selectedTeamMembers)
            {
                if(allies.Contains(teamMember.GetComponent<TeamMember>())) SelectedAllies.Add(teamMember);
            }
        }

        [Server]
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
