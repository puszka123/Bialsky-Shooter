using BialskyShooter.ClassSystem;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.MovementModule
{
    [RequireComponent(typeof(CreatureStats))]
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : NetworkBehaviour
    {
        [Inject] CreatureStats creatureStats = null;
        [Inject] Rigidbody rb = null;

        [SerializeField] float brakeFactor = 0.1f;
        [SerializeField] float moveFactor = 0.5f;      

        #region Server

        
        Vector3 ComputeMoveForce(Vector2 moveVector)
        {
            Vector3 moveForce = Vector3.zero;
            moveForce.x += moveVector.x * moveFactor * creatureStats.Agility.value;
            moveForce.z += moveVector.y * moveFactor * creatureStats.Agility.value;

            return moveForce;
        }

        
        public void Move(Vector2 moveVector, float fixedDeltaTime)
        {
            Vector3 moveForce = ComputeMoveForce(moveVector);
            Move(moveForce, fixedDeltaTime);
        }

        internal void StopMove()
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
        }

        [TargetRpc]
        public void TargetMove(Vector3 moveForce, float fixedDeltaTime)
        {
            Move(moveForce, fixedDeltaTime);
        }

        public void Move(Vector3 moveForce, float fixedDeltaTime)
        {
            rb.MovePosition(transform.position + moveForce * fixedDeltaTime);
        }

        public void Move(Vector3 moveForce)
        {
            rb.MovePosition(transform.position + moveForce);
        }

        public void Rotate(Vector3 lookAt)
        {
            lookAt.y = transform.position.y;
            var forward = lookAt - transform.position;
            var rotation = Quaternion.LookRotation(forward, Vector3.up);
            rb.MoveRotation(rotation);
        }

        [Server]
        public void Rotate(float angle)
        {
            var rotation = Quaternion.Euler(0, angle, 0);
            rb.MoveRotation(rb.rotation * rotation);
        }

        #endregion
    }
}