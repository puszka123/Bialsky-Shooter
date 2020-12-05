using System.Collections;
using System.Collections.Generic;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using UnityEngine;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(EnemySight))]
    [RequireComponent(typeof(SkillUser))]
    public class EnemyAI : NetworkBehaviour
    {
        EnemySight enemySight;
        AIMovement aiMovement;
        SkillUser skillUser;
        GameObject player;

        #region Server

        [ServerCallback]
        private void Start()
        {
            aiMovement = GetComponent<AIMovement>();
            enemySight = GetComponent<EnemySight>();
            skillUser = GetComponent<SkillUser>();
        }

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