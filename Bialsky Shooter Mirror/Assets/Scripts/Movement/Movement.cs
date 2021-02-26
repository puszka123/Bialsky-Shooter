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

        [Server]
        Vector3 ComputeMoveForce(Vector2 moveVector)
        {
            Vector3 moveForce = Vector3.zero;
            if (moveVector.x == 0f || Mathf.Sign(rb.velocity.x) != Mathf.Sign(moveVector.x)) //brake
            {
                moveForce.x = -rb.velocity.x * brakeFactor * creatureStats.Agility.value;
            }
            if (moveVector.y == 0f || Mathf.Sign(rb.velocity.z) != Mathf.Sign(moveVector.y)) //brake
            {
                moveForce.z = -rb.velocity.z * brakeFactor * creatureStats.Agility.value;
            }
            moveForce.x += moveVector.x * moveFactor * creatureStats.Agility.value;
            moveForce.z += moveVector.y * moveFactor * creatureStats.Agility.value;

            return moveForce;
        }

        [Server]
        public void Move(Vector2 moveVector)
        {
            Vector3 moveForce = ComputeMoveForce(moveVector);
            Move(moveForce);
        }

        internal void StopMove()
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
        }

        [Server]
        public void Move(Vector3 moveForce)
        {
            rb.AddForce(moveForce, ForceMode.Force);
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(
                rb.velocity.magnitude,
                0f,
                moveFactor * creatureStats.Agility.value);
        }

        [Server]
        public void MoveForce(Vector3 force)
        {
            rb.AddForce(force, ForceMode.Force);
        }

        [Server]
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
