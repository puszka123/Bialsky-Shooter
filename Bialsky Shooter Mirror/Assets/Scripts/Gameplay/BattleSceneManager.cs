using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace BialskyShooter.Gameplay
{
    public class BattleSceneManager : NetworkBehaviour
    {
        public static string BattleMapPrefix = "Map_Battle";

        public enum Priority
        {
            TeamUp = 0,
            Spawn = 1,
            GenerateInventory = 2,
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            LoadSceneContent();
        }


        void LoadSceneContent()
        {
            var runnables = FindObjectsOfType<Runnable>().SelectMany(r => r.GetRunnables());
            runnables = runnables.OrderBy(r => r.Priority());
            foreach (var runnable in runnables)
            {
                runnable.Run();
            }
        }
    }
}