using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class ResourcesStore : NetworkBehaviour
    {
        public static event Action<Resource> onResourceChanged;
        SyncDictionary<ResourceType, Resource> resources = new SyncDictionary<ResourceType, Resource>();
        

        private void Start()
        {
            foreach (var item in resources)
            {
                onResourceChanged?.Invoke(item.Value);
            }
        }

        public override void OnStartServer()
        {
            resources[ResourceType.Gold] = new Resource { Amount = 0f, ResourceType = ResourceType.Gold, DisplayName = "Gold" };
            resources[ResourceType.Stone] = new Resource { Amount = 0f, ResourceType = ResourceType.Stone, DisplayName = "Stone" };
            resources[ResourceType.Wood] = new Resource { Amount = 0f, ResourceType = ResourceType.Wood, DisplayName = "Wood" };
        }

        public Resource GetResource(ResourceType resourceType)
        {
            return resources[resourceType];
        }

        public IEnumerable<ResourceType> GetAllResourceTypes()
        {
            return resources.Keys;
        }

        public bool Affordable(IList<Resource> resources)
        {
            foreach (var resource in resources)
            {
                if (!this.resources.ContainsKey(resource.ResourceType)) return false;
                if (this.resources[resource.ResourceType].Amount < resource.Amount) return false;
            }
            return true;
        }

        #region server



        [Server]
        public void GainResource(Resource resource)
        {
            resources[resource.ResourceType].Amount += resource.Amount;
            TargetGainResource(resources[resource.ResourceType]);
        }

        [Server]
        public void ExtractResource(Resource resource)
        {
            resources[resource.ResourceType].Amount -= resource.Amount;
            TargetExtractResource(resources[resource.ResourceType]);
        }

        [Server]
        public void ExtractResources(IList<Resource> resources)
        {
            foreach (var resource in resources)
            {
                ExtractResource(resource);
            }
        }

        #endregion

        #region client

        [TargetRpc]
        void TargetGainResource(Resource resource)
        {
            if (!hasAuthority) return;
            onResourceChanged?.Invoke(resource);
        }

        [TargetRpc]
        void TargetExtractResource(Resource resource)
        {
            if (!hasAuthority) return;
            onResourceChanged?.Invoke(resource);
        }

        #endregion
    }
}
