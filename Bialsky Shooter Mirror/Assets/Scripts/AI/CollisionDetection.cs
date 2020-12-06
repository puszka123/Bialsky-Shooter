using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BialskyShooter.AI
{
    public class CollisionDetection : NetworkBehaviour
    {
        public static int angleIncrease = 10;
        [SerializeField] Transform center = default;

        #region Server

        [Server]
        public void UpdateCollisions(out float frontDist, out float rightDist, out float leftDist)
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
                direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                if (Physics.Raycast(center.position, center.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < frontDist)
                    {
                        frontDist = hit.distance;
                    }
                    //Debug.DrawRay(center.position, center.TransformDirection(direction) * hit.distance, Color.blue, 1f);
                }
            }
            for (int i = 60; i <= 90; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(-Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                if (Physics.Raycast(center.position, center.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < frontDist)
                    {
                        frontDist = hit.distance;
                    }
                    //Debug.DrawRay(center.position, center.TransformDirection(direction) * hit.distance, Color.blue, 1f);
                }
            }

            for (int i = 0; i <= 60; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                if (Physics.Raycast(center.position, center.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < rightDist)
                    {
                        rightDist = hit.distance;
                    }
                    //Debug.DrawRay(center.position, center.TransformDirection(direction) * hit.distance, Color.red, 1f);
                }
            }
            for (int i = 0; i <= 60; i += angleIncrease)
            {
                angle = Mathf.Deg2Rad * i;
                direction = new Vector3(-Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                if (Physics.Raycast(center.position, center.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < leftDist)
                    {
                        leftDist = hit.distance;
                    }
                    //Debug.DrawRay(center.position, center.TransformDirection(direction) * hit.distance, Color.green, 1f);
                }
            }
        }

        #endregion
    }
}