using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BialskyShooter.Multiplayer
{
    public class MyNetworkManager : NetworkManager
    {
        [SerializeField] GameObject humanEnemyPrefab;

        #region Server

        public override void OnServerSceneChanged(string sceneName)
        {
            base.OnServerSceneChanged(sceneName);
            if (SceneManager.GetActiveScene().name.StartsWith("Map"))
            {
                var enemyInstance = Instantiate(humanEnemyPrefab, GetStartPosition().position, Quaternion.identity);
                NetworkServer.Spawn(enemyInstance);
            }
        }

        #endregion

        #region Client

        #endregion
    }
}
