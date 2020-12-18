using BialskyShooter.ClassSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Movement
{
    [RequireComponent(typeof(Movement))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Inject] Movement movement;

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
