using System.Collections;
using System.Collections.Generic;
using BialskyShooter.Movement;
using Mirror;
using UnityEngine;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(EnemySight))]
    public class EnemyAI : NetworkBehaviour
    {
        EnemySight enemySight;
        AIMovement aiMovement;
        GameObject player;

        #region Server

        [ServerCallback]
        private void Start()
        {
            aiMovement = GetComponent<AIMovement>();
            enemySight = GetComponent<EnemySight>();
        }

        [ServerCallback]
        void Update()
        {
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");

            if (enemySight.CanSeePlayer())
            {
                aiMovement.Move(player.transform.position);
            }

        }

        #endregion
    }
}