using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Core
{
    public class PlayerCameraSpawner : NetworkBehaviour
    {
        #region Client

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!hasAuthority) return;
            var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform;
        }

        #endregion
    }
}

