using BialskyShooter.Combat;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static BialskyShooter.AI.Command;

namespace BialskyShooter.AI
{
    public class CommandGiver : NetworkBehaviour
    {
        [Serializable]
        public class SerializedVector2
        {
            public float x, y;
            public SerializedVector2(Vector2 vector2) { x = vector2.x; y = vector2.y; }
            public Vector2 ToVector2() { return new Vector2(x, y); }
        }

        AllyBoxSelector allyBoxSelector;
        LayerMask layerMask = default;

        private void Awake()
        {
            layerMask = LayerMask.GetMask("Object");
            allyBoxSelector = FindObjectOfType<AllyBoxSelector>();
        }

        #region Server

        [Command]
        public void CmdCommandAllies(List<NetworkIdentity> allies, CommandId commandId, Vector2 mousePosition)
        {
            CommandAllies(allies, commandId, mousePosition);
        }

        [Server]
        void CommandAllies(List<NetworkIdentity> allies, CommandId commandId, Vector2 mousePosition)
        {
            switch (commandId)
            {
                case CommandId.Fight:
                    var ray = GetMyCamera().ScreenPointToRay(mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.transform.TryGetComponent<CombatTarget>(out CombatTarget combatTarget))
                        {
                            SendCommandToAllies(allies, new CommandArgs(commandId, hit.transform.gameObject));
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        [Server]
        Camera GetMyCamera()
        {
            return Camera.main;
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
            if(Mouse.current.rightButton.wasPressedThisFrame)
            {

                var mousePosition = Mouse.current.position.ReadValue();
                CmdCommandAllies(allyBoxSelector.SelectedAllies.ToList(), CommandId.Fight, mousePosition);
            }
        }
        #endregion
    }
}
