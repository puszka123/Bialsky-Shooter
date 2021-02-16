using BialskyShooter.ResourcesModule.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingCreator : NetworkBehaviour
    {
        

        #region server

        [Command]
        public void CmdPlaceBuilding(Guid buildingId, Vector3 spawnPosition)
        {
            
        }

        

        #endregion

        

    }
}
