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
using BialskyShooter.InventoryModule;
using BialskyShooter.EquipmentSystem;

namespace BialskyShooter.ManagementModule
{
    [RequireComponent(typeof(Health))]
    public class LoseManager : NetworkBehaviour
    {
        #region Server

        public override void OnStartServer()
        {
            GetComponent<Health>().serverOnCreatureLose += ServerOnCreatureLose;
        }

        public override void OnStopServer()
        {
            GetComponent<Health>().serverOnCreatureLose -= ServerOnCreatureLose;
        }

        [Server]
        void ServerOnCreatureLose()
        {
            if (TryGetComponent(out EquipmentController equipmentController))
            {
                equipmentController.ServerUnequipAll();
            }
            OnCreatureLose();
        }

        #endregion


        #region Client

        public override void OnStartClient()
        {
            if (NetworkServer.active) return;
            GetComponent<Health>().clientOnCreatureLose += ClientOnCreatureLose;
            if (GetComponent<Health>().IsDefeated) OnCreatureLose();
        }

        public override void OnStopClient()
        {
            if (NetworkServer.active) return;
            GetComponent<Health>().clientOnCreatureLose -= ClientOnCreatureLose;
        }

        [Client]
        void ClientOnCreatureLose()
        {
            if (TryGetComponent(out EquipmentController equipmentController))
            {
                equipmentController.ClientUnequipAll();
            }
            OnCreatureLose();
        }

        #endregion

        void OnCreatureLose()
        {
            if (TryGetComponent(out EnemyAI enemyAI)) enemyAI.enabled = false;
            if (TryGetComponent(out AIMovement aiMovement)) aiMovement.enabled = false;
            if (TryGetComponent(out SkillUser skillUser)) skillUser.enabled = false;
            if (TryGetComponent(out PlayerMoveController ctrl)) ctrl.enabled = false;
            if (TryGetComponent(out PlayerSkillController skillCtrl)) skillCtrl.enabled = false;
            if (TryGetComponent(out CombatTarget target)) target.enabled = false;
            if (TryGetComponent(out StateMachine stateMachine)) stateMachine.enabled = false;
            if (TryGetComponent(out ActionScheduler actionScheduler))
            {
                actionScheduler.TryCancelCurrentAction();
                actionScheduler.enabled = false;
            }
            if (TryGetComponent(out TeamMember teamMember))
            {
                FindObjectOfType<TeamManager>().RemoveFromTeam(teamMember);
                teamMember.enabled = false;
            }
            if (TryGetComponent(out BoxCollider boxCollider))
            {
                boxCollider.isTrigger = true;
            }
            if (TryGetComponent(out CapsuleCollider capsuleCollider))
            {
                capsuleCollider.isTrigger = true;
            }
            if (TryGetComponent(out Rigidbody rigidbody)) rigidbody.isKinematic = true;
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null) renderer.material.color = Color.blue;
            if(GetComponent<Inventory>() != null) gameObject.AddComponent<LootTarget>();
        }
    }
}
