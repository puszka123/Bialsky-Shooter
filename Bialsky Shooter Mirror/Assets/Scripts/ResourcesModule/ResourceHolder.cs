using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class ResourceHolder : NetworkBehaviour
    {
        [SerializeField] float capacity = 5f;
        [SerializeField] ResourceType resourceType = ResourceType.Gold;
        Resource holdingResource;

        public float Capacity { get { return capacity; } }
        public bool IsHolding { get { return holdingResource != null; } }

        #region server

        [Server]
        public void GainResource(Resource resource)
        {
            holdingResource = resource;
        }

        [Server]
        public Resource GiveBackResource()
        {
            var resourceToGiveBack = holdingResource;
            holdingResource = null;
            return resourceToGiveBack;
        }

        #endregion
    }
}