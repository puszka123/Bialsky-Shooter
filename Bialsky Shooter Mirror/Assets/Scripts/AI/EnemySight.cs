using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class EnemySight : NetworkBehaviour
    {
        GameObject player;

        #region Server

        [Server]
        public bool CanSeePlayer()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                if (player == null) return false;
            }
            LayerMask layerMask = LayerMask.GetMask("Object");
            RaycastHit hit;
            bool result = Physics.Linecast(transform.position, player.transform.position, out hit, layerMask);
            if(!result || (result && (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))))
            {

                return true;
            }
            return false;
        }

        #endregion
    }
}