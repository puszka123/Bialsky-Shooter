using BialskyShooter.Combat;
using BialskyShooter.ResourcesModule;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static BialskyShooter.AI.Command;

namespace BialskyShooter.AI
{
    public class CommandGiver : NetworkBehaviour
    {
        [Inject] AllySelector allySelector;

        LayerMask layerMask = default;

        private void Awake()
        {
            layerMask = LayerMask.GetMask("Object", "Terrain");
        }

        #region Server

        [Command]
        public void CmdCommandAlliesMove(Vector3 movePosition)
        {
            var idk = allySelector.gameObject.GetInstanceID();
            if (allySelector.SelectedAllies != null && allySelector.SelectedAllies.Count > 0)
            {
                CommandAllies(allySelector.SelectedAllies.ToList(), movePosition);
            }
        }

        [Command]
        public void CmdCommandAlliesTarget(NetworkIdentity target)
        {
            if (allySelector.SelectedAllies != null && allySelector.SelectedAllies.Count > 0)
            {
                CommandAllies(allySelector.SelectedAllies.ToList(), target);
            }
        }

        [Server]
        void CommandAllies(List<NetworkIdentity> allies, Vector3 movePosition)
        {
            SendCommandToAllies(allies, new CommandArgs(ActionId.Move, movePosition));
        }

        [Server]
        void CommandAllies(List<NetworkIdentity> allies, NetworkIdentity target)
        {
            var actionId = ActionId.Fight; //to do
            if (target.GetComponent<ResourceSource>() != null) actionId = ActionId.Extract;
            SendCommandToAllies(allies, new CommandArgs(actionId, target.gameObject));
        }

        [Server]
        void SendCommandToAllies(List<NetworkIdentity> allies, CommandArgs args)
        {
            foreach (var ally in allies)
            {
                ally.GetComponent<CommandReceiver>().ReceiveCommand(args);
            }
        }

        #endregion

        #region Client

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) return;
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                var ray = Camera.main.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        CmdCommandAlliesMove(hit.point);
                    }
                    else if (hit.transform.TryGetComponent(out NetworkIdentity networkIdentity))
                    {
                        CmdCommandAlliesTarget(networkIdentity);
                    }
                }
            }
        }
        #endregion
    }
}
