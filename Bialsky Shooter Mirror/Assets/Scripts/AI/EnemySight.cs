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
            LayerMask layerMask = LayerMask.GetMask("Object", "Terrain");
            RaycastHit hit;
            var myHeight = 0f;
            if(GetComponent<CapsuleCollider>() != null) myHeight = GetComponent<CapsuleCollider>().height;
            else myHeight = GetComponent<BoxCollider>().size.y;
            var playerHeight = player.GetComponent<CapsuleCollider>().height;
            var myPosition = new Vector3(transform.position.x, transform.position.y + myHeight, transform.position.z);
            var playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + playerHeight, player.transform.position.z);
            bool result = Physics.Linecast(myPosition, playerPosition, out hit, layerMask);
            if(!result || (result && (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}