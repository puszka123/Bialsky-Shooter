using BialskyShooter.Movement;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BialskyShooter.Control
{
    [RequireComponent(typeof(CreatureMovement))]
    public class PlayerController : NetworkBehaviour
    {
        Vector2 previousInput;
        CreatureMovement movement;

        private void Start()
        {
            movement = GetComponent<CreatureMovement>();
        }

        #region Client

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Controls controls = new Controls();
            controls.Player.MovePlayer.performed += SetPreviousInput;
            controls.Player.MovePlayer.canceled += SetPreviousInput;
            controls.Enable();
        }

        [Client]
        void SetPreviousInput(InputAction.CallbackContext callbackContext)
        {
            previousInput = callbackContext.ReadValue<Vector2>();
        }

        [ClientCallback]
        void FixedUpdate()
        {
            if (!hasAuthority) return;
            movement.CmdMove(previousInput);
        }

        #endregion
    }
}
