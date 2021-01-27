using System.Collections;
using System.Collections.Generic;
using BialskyShooter.AI.Pathfinding;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using UnityEngine;
using Zenject;
using System.Linq;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(EnemySight))]
    [RequireComponent(typeof(SkillUser))]
    public class EnemyAI : NetworkBehaviour
    {
        [Inject] EnemySight enemySight = null;
        [Inject] AIMovement aiMovement = null;
        [Inject] SkillUser skillUser = null;
        [Inject] PathFinder pathFinder = null;
        GameObject player;
        Vector3[] path;
        int index = 0;

        #region Server

        [ServerCallback]
        void Update()
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player");

            if (enemySight.CanSeePlayer())
            {
                path = null;
                aiMovement.Move(player.transform.position);
                skillUser.UseRandomSkill();
            }
            else
            {
                if (path == null || (index >= path.Length))
                {
                    print("New path");
                    path = pathFinder.FindPath(player.transform.position).ToArray();
                    index = 0;
                }
                if ((index < path.Length) && Vector3.Distance(path[index], transform.position) <= 2f) ++index;
                if (index < path.Length) aiMovement.Move(path[index]);
            }
        }

        #endregion
    }
}