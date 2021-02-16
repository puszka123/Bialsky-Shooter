using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule.UI
{
    public class BuildingDisplay : MonoBehaviour 
    {
        [SerializeField] GameObject contentPanel;
        [SerializeField] GameObject buildingSlotPrefab;
        BuildingsStorage localBuildingsStorage;
        List<BuildingSlot> buildingSlots;

        void Awake()
        {
            buildingSlots = new List<BuildingSlot>();
        }

        void Update()
        {
            if (localBuildingsStorage == null) GetLocalBuildingsStorage();
            if(localBuildingsStorage != null && buildingSlots.Count == 0)
            {
                DisplayAvailableBuildings();
            }
        }

        

        void DisplayAvailableBuildings()
        {
            foreach (var buildingSlot in buildingSlots)
            {
                Destroy(buildingSlot.gameObject);
            }
            buildingSlots.Clear();
            foreach (var buildingPreview in localBuildingsStorage.BuildingPreviews)
            {
                var instance = Instantiate(buildingSlotPrefab, contentPanel.transform);
                var buildingSlot = instance.GetComponent<BuildingSlot>();
                buildingSlot.SetBuildingSlot(buildingPreview);
                buildingSlots.Add(buildingSlot);
            }
        }

        void GetLocalBuildingsStorage()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    localBuildingsStorage = player.GetComponent<BuildingsStorage>();
                    break;
                }
            }
        }
    }
}
