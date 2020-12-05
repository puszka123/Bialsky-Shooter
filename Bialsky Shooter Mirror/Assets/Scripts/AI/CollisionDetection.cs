using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BialskyShooter.AI
{
    public class CollisionDetection : NetworkBehaviour
    {
        public static int angleIncrease = 10;

        #region Server

        [Server]
        public static void UpdateCollisions(Transform transform, out float frontDist, out float rightDist, out float leftDist)
        {
            RaycastHit hit;

            float angle;
            Vector3 direction;
            LayerMask layerMask = LayerMask.GetMask("Object");
            frontDist = 20f;
            rightDist = 20f;
            leftDist = 20f;
            for (int i = 60; i <= 90; i+= angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < frontDist)
                    {
                        frontDist = hit.distance;
                    }
                    //Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, Color.blue);
                }
            }
            for (int i = 60; i <= 90; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(-Mathf.Cos(angle), 0, Mathf.Sin(angle));
                if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < frontDist)
                    {
                        frontDist = hit.distance;
                    }
                    //Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, Color.blue);
                }
            }

            for (int i = 0; i <= 60; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < rightDist)
                    {
                        rightDist = hit.distance;
                    }
                    //Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, Color.red);
                }
            }
            for (int i = 0; i <= 60; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(-Mathf.Cos(angle), 0, Mathf.Sin(angle));
                if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < leftDist)
                    {
                        leftDist = hit.distance;
                    }
                    //Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, Color.green);
                }
            }
        }

        #endregion
    }
}