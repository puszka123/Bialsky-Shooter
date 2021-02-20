using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    public class SpawnRequirements : NetworkBehaviour
    {
        [SerializeField] List<Resource> spawnCost;

        public IList<Resource> SpawnCost { get { return spawnCost; } }
    }
}