using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class EnemySight : NetworkBehaviour
    {
        #region Server

        [Server]
        public bool CanSeeTarget(GameObject target)
        {
            if (target == null) return false;
            LayerMask layerMask = LayerMask.GetMask("Object", "Terrain");
            RaycastHit hit;
            var myHeight = 0f;
            var targetHeight = 0f;
            if(GetComponent<CapsuleCollider>() != null) myHeight = GetComponent<CapsuleCollider>().height;
            else myHeight = GetComponent<BoxCollider>().size.y;
            if (target.GetComponent<CapsuleCollider>() != null) targetHeight = target.GetComponent<CapsuleCollider>().height;
            else targetHeight = target.GetComponent<BoxCollider>().size.y;


            var myPosition = new Vector3(transform.position.x, transform.position.y + myHeight, transform.position.z);
            var targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + targetHeight, target.transform.position.z);
            bool result = Physics.Linecast(myPosition, targetPosition, out hit, layerMask);
            if(!result || (result && (hit.transform.CompareTag("Creature") || hit.transform.CompareTag("PlayerCharacter"))))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}