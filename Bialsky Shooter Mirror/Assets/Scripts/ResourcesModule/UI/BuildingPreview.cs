using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    [Serializable]
    public class BuildingPreview
    {
        public Guid buildingId;
        public string buildingPreviewPrefabPath;
        public bool available;
        public string iconPath;
        public Vector3 spawnPosition;
    }
}
