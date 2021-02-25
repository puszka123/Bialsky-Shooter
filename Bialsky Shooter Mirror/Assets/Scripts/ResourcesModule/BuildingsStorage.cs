using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingsStorage : NetworkBehaviour
    {
        [SerializeField] List<Building> buildings;
        public IList<Building> AvailableBuildings { get; private set; }
        public SyncList<BuildingPreview> BuildingPreviews { get; private set; } = new SyncList<BuildingPreview>();

        private void Awake()
        {
            foreach (var building in buildings)
            {
                building.Init();
            }
            AvailableBuildings = new List<Building>(buildings);
        }

        private void Start()
        {
            UpdateBuildingPreviews();
        }

        [Server]
        void UpdateBuildingPreviews()
        {
            BuildingPreviews.Clear();
            foreach (var building in buildings)
            {
                BuildingPreviews.Add(new BuildingPreview { 
                    buildingId = building.Id,
                    buildingPreviewPrefabPath = building.BuildingConfig.BuildingPreviewPrefab.name,
                    available = AvailableBuildings.Select(b => b.Id).Contains(building.Id),
                    iconPath = building.BuildingConfig.Icon.name,
                });
            }
        }
    }
}