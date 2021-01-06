using BialskyShooter.Movement;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace BialskyShooter.Control
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMoveController : NetworkBehaviour
    {
        Vector2 previousInput;
        [Inject] PlayerMovement movement = null;

        #region Client

        public override void OnStartAuthority()
        {
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

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority) return;
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;
            movement.CmdRotate(hit.point);
        }

        #endregion
    }
}
