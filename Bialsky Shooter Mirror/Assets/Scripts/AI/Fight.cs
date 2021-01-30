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
        GameObject player;
        Vector3[] path;
        int index = 0;
        bool execute = false;

        private void Update()
        {
            if (!execute) return;
            TryToFindPlayer();
            if (enemySight.CanSeePlayer())
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

        private void TryToFindPlayer()
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player");
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
            path = pathFinder.FindPath(player.transform.position).ToArray();
            index = 0;
        }

        private void GoDirectlyToTarget()
        {
            aiMovement.Move(player.transform.position);
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