using BialskyShooter.ClassSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Movement
{
    [RequireComponent(typeof(CreatureStats))]
    [RequireComponent(typeof(Rigidbody))]
    public class CreatureMovement : NetworkBehaviour
    {
        [SerializeField] float brakeFactor = 0.1f;
        [SerializeField] float moveFactor = 0.5f;
        CreatureStats creatureStats;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            creatureStats = GetComponent<CreatureStats>();
        }

        #region Server

        [Command]
        public void CmdMove(Vector2 moveVector)
        {
            Vector3 moveForce = ComputeMoveForce(moveVector);
            rb.AddForce(moveForce, ForceMode.Force);
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(
                rb.velocity.magnitude, 
                0f,
                moveFactor * creatureStats.Agility.value);
        }

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

        #endregion
    }
}
