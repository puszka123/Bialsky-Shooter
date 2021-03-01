using BialskyShooter.ClassSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.MovementModule
{
    [RequireComponent(typeof(Movement))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Inject] Movement movement = null;

        #region Server

        [Command]
        public void CmdMove(Vector2 moveVector, float fixedDeltaTime)
        {
            movement.Move(moveVector, fixedDeltaTime);
        }

        [Command]
        public void CmdRotate(Vector3 lookAt)
        {
            movement.Rotate(lookAt);
        }

        #endregion

        public void ClientMove(Vector2 moveVector, float fixedDeltaTime)
        {
            movement.Move(moveVector, fixedDeltaTime);
        }

        public void ClientRotate(Vector3 lookAt)
        {
            movement.Rotate(lookAt);
        }
    }
}
