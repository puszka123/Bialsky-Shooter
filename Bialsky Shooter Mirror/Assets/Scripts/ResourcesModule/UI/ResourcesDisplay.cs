using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BialskyShooter.ResourcesModule.UI
{
    public class ResourcesDisplay : MonoBehaviour
    {
        [SerializeField] GameObject resourcesPanel;
        [SerializeField] GameObject resourceItemPrefab;
        ResourcesStore localResourcesStore;
        Dictionary<ResourceType, GameObject> resourceItems;
        bool initialized;

        private void Awake()
        {
            resourceItems = new Dictionary<ResourceType, GameObject>();
            ResourcesStore.onResourceChanged += OnResourceChanged;
        }

        private void OnDestroy()
        {
            ResourcesStore.onResourceChanged -= OnResourceChanged;
        }

        private void OnResourceChanged(Resource resource)
        {
            RedrawResource(resource);
        }

        private void Update()
        {
            if (localResourcesStore != null) return;
            GetLocalResourcesStore();
            if (localResourcesStore != null && !initialized)
            {
                InitPanel();
                RedrawResources();
            }
        }

        private void InitPanel()
        {
            foreach (var resourceType in localResourcesStore.GetAllResourceTypes())
            {
                var resourceItemInstance = Instantiate(resourceItemPrefab, resourcesPanel.transform);
                resourceItems[resourceType] = resourceItemInstance;
            }
        }

        private void RedrawResources()
        {
            foreach (var resourceType in localResourcesStore.GetAllResourceTypes())
            {
                SetResourceItem(resourceType, localResourcesStore.GetResource(resourceType));
            }
        }

        private void RedrawResource(Resource resource)
        {
            SetResourceItem(resource.ResourceType, resource);
        }

        private void SetResourceItem(ResourceType resourceType, Resource resource)
        {
            if (!resourceItems.ContainsKey(resourceType)) return;
            resourceItems[resourceType].transform.GetChild(0).GetComponent<TMP_Text>().text = resource.DisplayName;
            resourceItems[resourceType].transform.GetChild(1).GetComponent<TMP_Text>().text = resource.Amount.ToString();
        }

        private void GetLocalResourcesStore()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    localResourcesStore = player.GetComponent<ResourcesStore>();
                    break;
                }
            }
        }

    }
}