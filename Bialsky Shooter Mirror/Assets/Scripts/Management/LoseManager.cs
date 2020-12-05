using BialskyShooter.AI;
using BialskyShooter.Combat;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using BialskyShooter.Control;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ManagementModule
{
    [RequireComponent(typeof(Health))]
    public class LoseManager : NetworkBehaviour
    {
        #region Server

        public override void OnStartServer()
        {
            GetComponent<Health>().serverOnCreatureLose += OnCreatureLose;
        }

        public override void OnStopServer()
        {
            GetComponent<Health>().serverOnCreatureLose -= OnCreatureLose;
        }

        void OnCreatureLose()
        {
            if (TryGetComponent<EnemyAI>(out EnemyAI enemyAI)) enemyAI.enabled = false;
            if (TryGetComponent<AIMovement>(out AIMovement aiMovement)) aiMovement.enabled = false;
            if (TryGetComponent<SkillUser>(out SkillUser skillUser)) skillUser.enabled = false;
            if (TryGetComponent<PlayerMoveController>(out PlayerMoveController ctrl)) ctrl.enabled = false;
            if (TryGetComponent<PlayerSkillController>(out PlayerSkillController skillCtrl)) skillCtrl.enabled = false;
            if (TryGetComponent<CombatTarget>(out CombatTarget target)) target.enabled = false;
            if (TryGetComponent<BoxCollider>(out BoxCollider boxCollider)) boxCollider.enabled = false;
            if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider)) capsuleCollider.enabled = false;
            if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) rigidbody.isKinematic = true;
            var renderer = GetComponentInChildren<Renderer>();
            if(renderer != null) renderer.material.color = Color.blue;
        }

        #endregion
    }
}
