using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "ScriptableObjects/Resources/BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] GameObject buildingPreviewPrefab;
        [SerializeField] GameObject buildingPrefab;
        [SerializeField] Sprite icon;
        public GameObject BuildingPreviewPrefab { get { return buildingPreviewPrefab; } }
        public GameObject BuildingPrefab { get { return buildingPrefab; } }
        public Sprite Icon { get { return icon; } }
    }
}
