using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class ResourcesStore : NetworkBehaviour
    {
        public event Action<Resource> onResourceChanged;
        SyncDictionary<ResourceType, Resource> resources = new SyncDictionary<ResourceType, Resource>();
        

        private void Awake()
        {
            resources[ResourceType.Gold] = new Resource { Amount = 0f, ResourceType = ResourceType.Gold, DisplayName = "Gold" };
            resources[ResourceType.Stone] = new Resource { Amount = 0f, ResourceType = ResourceType.Stone, DisplayName = "Stone" };
            resources[ResourceType.Wood] = new Resource { Amount = 0f, ResourceType = ResourceType.Wood, DisplayName = "Wood" };
        }

        private void Start()
        {
            foreach (var item in resources)
            {
                onResourceChanged?.Invoke(item.Value);
            }
        }

        [Client]
        public Resource GetResource(ResourceType resourceType)
        {
            return resources[resourceType];
        }

        [Client]
        public IEnumerable<ResourceType> GetAllResourceTypes()
        {
            return resources.Keys;
        }

        [TargetRpc]
        public void TargetGainResource(Resource resource)
        {
            GainResource(resource);
        }


        [Client]
        public void GainResource(Resource resource)
        {
            resources[resource.ResourceType].Amount += resource.Amount;
            onResourceChanged?.Invoke(resources[resource.ResourceType]);
        }
    }
}
