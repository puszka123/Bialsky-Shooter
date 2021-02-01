using BialskyShooter.AI.Pathfinding;
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
    public class Fight : NetworkBehaviour, IAction
    {
        [Inject] EnemySight enemySight = null;
        [Inject] AIMovement aiMovement = null;
        [Inject] PathFinder pathFinder = null;
        [Inject] SkillUser skillUser = null;
        [Inject] Aggravate aggravate = null;
        Vector3[] path;
        int index = 0;
        bool execute = false;

        private void Update()
        {
            if (!execute || aggravate.NearbyTarget == null) return;
            if (enemySight.CanSeeTarget(aggravate.NearbyTarget))
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

        private void ClearPath()
        {
            path = null;
        }

        private void TryToFindPath()
        {
            if (path == null || (index >= path.Length))
            {
                FindPath();
            }
        }

        private void FollowPathToFindTarget()
        {
            if ((index < path.Length) && Vector3.Distance(path[index], transform.position) <= 2f) ++index;
            if (index < path.Length) aiMovement.Move(path[index]);
        }

        private void FindPath()
        {
            path = pathFinder.FindPath(aggravate.NearbyTarget.transform.position).ToArray();
            index = 0;
        }

        private void GoDirectlyToTarget()
        {
            aiMovement.Move(aggravate.NearbyTarget.transform.position);
            skillUser.UseRandomSkill();
        }

        public void Cancel()
        {
            execute = false;
            aiMovement.StopMove();
        }

        public void Execute()
        {
            execute = true;
        }

        public bool CanExecute()
        {
            return !Mathf.Approximately(aggravate.AggravateValue, 0f);
        }
    }
}