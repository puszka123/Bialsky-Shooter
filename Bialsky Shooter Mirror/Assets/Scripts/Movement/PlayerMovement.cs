using BialskyShooter.ClassSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Movement
{
    [RequireComponent(typeof(Movement))]
    public class PlayerMovement : NetworkBehaviour
    {
        Movement movement;

        private void Start()
        {
            movement = GetComponent<Movement>();
        }

        #region Server

        [Command]
        public void CmdMove(Vector2 moveVector)
        {
            movement.Move(moveVector);
        }

        [Command]
        public void CmdRotate(Vector3 lookAt)
        {
            movement.Rotate(lookAt);
        }

        #endregion
    }
}
