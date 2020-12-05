using System.Collections;
using System.Collections.Generic;
using BialskyShooter.AI;
using Mirror;
using UnityEngine;

namespace BialskyShooter.Movement
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(CollisionDetection))]
    public class AIMovement : NetworkBehaviour
    {
        [SerializeField] float ApproximateDistance = 5f;
        MovementSystem movementSystem;
        Movement movement;
        CollisionDetection collisionDetection;
        

        #region Server

        [ServerCallback]
        void Start()
        {
            movementSystem = new MovementSystem();
            movement = GetComponent<Movement>();
            collisionDetection = GetComponent<CollisionDetection>();
        }

        [Server]
        bool ShouldMove(Vector3 destination)
        {
            return ApproximateDistance < Vector3.Distance(transform.position, destination);
        }

        [Server]
        float ComputeAngle(Vector3 destination)
        {
            float goalAngle = GetBearing(transform.position, destination);
            collisionDetection.UpdateCollisions(out float frontDist, out float rightDist, out float leftDist);
            movementSystem.Calculate(frontDist, rightDist, leftDist, out float avoidanceAngle);
            float avoidanceWeight = 1.0f;
            float goalWeight = 1.0f;
            float finalAngle = goalWeight * goalAngle + avoidanceWeight * avoidanceAngle;
            return finalAngle;
        }

        [Server]
        float GetBearing(Vector3 locationA, Vector3 locationB)
        {

            return Vector3.SignedAngle(locationB - locationA, transform.forward, Vector3.up) * (-1f);
        }

        [Server]
        public void Move(Vector3 destination)
        {
            if (!ShouldMove(destination)) return;
            movement.Rotate(ComputeAngle(destination));
            //var moveVector3 = destination - transform.position;
            var moveVector3 = transform.forward;
            var moveVector2 = new Vector2(moveVector3.x, moveVector3.z);
            movement.Move(moveVector2);
        }

        #endregion
    }
}