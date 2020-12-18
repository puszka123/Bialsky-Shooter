using System.Collections;
using System.Collections.Generic;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(EnemySight))]
    [RequireComponent(typeof(SkillUser))]
    public class EnemyAI : NetworkBehaviour
    {
        [Inject] EnemySight enemySight;
        [Inject] AIMovement aiMovement;
        [Inject] SkillUser skillUser;
        GameObject player;

        #region Server

        [ServerCallback]
        void Update()
        {
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");

            if (enemySight.CanSeePlayer())
            {
                aiMovement.Move(player.transform.position);
                skillUser.UseRandomSkill();
            }

        }

        #endregion
    }
}