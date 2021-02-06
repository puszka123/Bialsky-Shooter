using BialskyShooter.AI.Pathfinding;
using BialskyShooter.Combat;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(EnemySight))]
    [RequireComponent(typeof(Aggravate))]
    public class Fight : NetworkBehaviour, ICommand
    {
        [Inject] EnemySight enemySight = null;
        [Inject] AIMovement aiMovement = null;
        [Inject] PathFinder pathFinder = null;
        [Inject] SkillUser skillUser = null;
        [Inject] Aggravate aggravate = null;
        Vector3[] path;
        int index = 0;
        bool execute = false;
        GameObject commandTarget;
        Health commandTargetHealth;
        GameObject currentTarget;

        [ServerCallback]
        private void Update()
        {
            currentTarget = commandTarget != null ? commandTarget : aggravate.NearbyTarget;
            if (!execute || currentTarget == null) return;
            if (enemySight.CanSeeTarget(currentTarget))
            {
                ClearPath();
                GoDirectlyToTarget();
            }
            else
            {
                TryToFindPath();
                FollowPathToFindTarget();
            }
        }

        [Server]
        private void ClearPath()
        {
            path = null;
        }

        [Server]
        private void TryToFindPath()
        {
            if (path == null || (index >= path.Length))
            {
                FindPath();
            }
        }

        [Server]
        private void FollowPathToFindTarget()
        {
            if ((index < path.Length) && Vector3.Distance(path[index], transform.position) <= 2f) ++index;
            if (index < path.Length) aiMovement.Move(path[index]);
        }

        [Server]
        private void FindPath()
        {
            path = pathFinder.FindPath(currentTarget.transform.position).ToArray();
            index = 0;
        }

        [Server]
        private void GoDirectlyToTarget()
        {
            aiMovement.Move(currentTarget.transform.position);
            skillUser.UseRandomSkill();
        }

        [Server]
        public void Cancel()
        {
            execute = false;
            aiMovement.StopMove();
        }

        [Server]
        public void Execute()
        {
            execute = true;
        }

        [Server]
        public bool CanExecute()
        {
            return !Mathf.Approximately(aggravate.AggravateValue, 0f);
        }

        [Server]
        public bool Completed()
        {
            return commandTargetHealth.IsDefeated;
        }

        [Server]
        public void SetTarget(dynamic target)
        {
            commandTarget = target;
            commandTargetHealth = commandTarget.GetComponent<Health>();
        }

        public Command.CommandId GetCommandId()
        {
            return Command.CommandId.Fight;
        }
    }
}