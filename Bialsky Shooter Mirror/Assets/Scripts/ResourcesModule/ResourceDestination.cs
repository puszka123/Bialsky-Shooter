using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class ResourceDestination : NetworkBehaviour
    {
        [SerializeField] ResourceType resourceType;
        public ResourcesStore ResourceStore { get; set; }
        public ResourceType ResourceType { get { return resourceType; } }

        [Server]
        public void GainResource(Resource resource)
        {
            if (resource.ResourceType != resourceType) throw new System.Exception($"{name}: Invalid resource type: {resource.ResourceType} != {resourceType}");
            ResourceStore.GainResource(resource);
        }
    }
}
