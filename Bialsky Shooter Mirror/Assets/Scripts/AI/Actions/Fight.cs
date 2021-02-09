using BialskyShooter.AI.Pathfinding;
using BialskyShooter.Combat;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using Mirror;
using System.Linq;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "FightCommand", menuName = "ScriptableObjects/AI/Commands/Fight")]
    public class Fight : Action, ICommand
    {
        EnemySight enemySight = null;
        AIMovement aiMovement = null;
        PathFinder pathFinder = null;
        SkillUser skillUser = null;
        Aggravate aggravate = null;
        Vector3[] path;
        int index = 0;
        bool execute = false;
        GameObject commandTarget;
        Health commandTargetHealth;
        GameObject currentTarget;
        Transform self;

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
            if ((index < path.Length) && Vector3.Distance(path[index], self.position) <= 2f) ++index;
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
        public override void Cancel()
        {
            execute = false;
            commandTarget = null;
            aiMovement.StopMove();
        }

        [Server]
        public override void Execute(MonoBehaviour executor)
        {
            execute = true;
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
        public override bool CanExecute()
        {
            return !Mathf.Approximately(aggravate.AggravateValue, 0f);
        }
        [Server]
        public override ActionId GetActionId()
        {
            return ActionId.Fight;
        }

        [Server]
        public override bool Executing()
        {
            return execute;
        }

        [Server]
        public override IAction SetSelf(GameObject self)
        {
            aiMovement = self.GetComponent<AIMovement>();
            aggravate = self.GetComponent<Aggravate>();
            pathFinder = self.GetComponent<PathFinder>();
            skillUser = self.GetComponent<SkillUser>();
            enemySight = self.GetComponent<EnemySight>();
            this.self = self.transform;
            return this;
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
    }
}