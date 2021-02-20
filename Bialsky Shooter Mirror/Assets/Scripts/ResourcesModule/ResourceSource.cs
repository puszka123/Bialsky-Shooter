using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class ResourceSource : Building
    {
        [SerializeField] Resource resource = default;

        [Server]
        public void Extract(float amount, NetworkIdentity extractor)
        { 
            var extractedResource = resource.Extract(amount);
            extractor.GetComponent<ResourceHolder>().GainResource(extractedResource);
        }
    }
}
