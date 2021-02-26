using BialskyShooter.MovementModule;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "PatrolAction", menuName = "ScriptableObjects/AI/Actions/Patrol")]
    public class Patrol : Action, IAction
    {
        [SerializeField] float patrolTime = 5f;
        [SerializeField] float patrolRange = 5f;
        [SerializeField] int newPatrolPlaceAttempts = 30;
        [SerializeField] float distanceToPatrolPosition = 2f;
        AIMovement aiMovement = null;
        Aggravate aggravate = null;
        Memory memory = null;
        bool patrol = false;
        bool movingToNewPosition = false;
        Vector3 patrolPosition;
        bool execute = false;
        Transform self;

        [Server]
        IEnumerator PatrolArea()
        {
            aiMovement.StopMove();
            patrol = true;
            yield return new WaitForSeconds(Random.Range(0f, patrolTime+1));
            patrol = false;
            UpdatePatrolPosition();
        }

        [Server]
        private void UpdatePatrolPosition()
        {
            for (int i = 0; i < newPatrolPlaceAttempts; i++)
            {
                patrolPosition = GetPatrolPosition();
                if (Vector3.Distance(memory.SpawnerPosition, patrolPosition) <= memory.SpawnRange)
                {
                    movingToNewPosition = true;
                    break;
                }
            }
            if(!movingToNewPosition)
            {
                patrolPosition = memory.SpawnerPosition;
                movingToNewPosition = true;
            }
        }

        [Server]
        private Vector3 GetPatrolPosition()
        {
            var seed = Random.insideUnitCircle * patrolRange;
            var newPatrolPosition = new Vector3(self.position.x + seed.x, self.position.y, self.position.z + seed.y);
            return newPatrolPosition;
        }

        [Server]
        public override void Execute(MonoBehaviour executor)
        {
            execute = true;
            if (movingToNewPosition)
            {
                aiMovement.Move(patrolPosition);
                movingToNewPosition = Vector3.Distance(self.position, patrolPosition) > distanceToPatrolPosition;
            }
            else if (!patrol)
            {
                executor.StartCoroutine(PatrolArea());
            }
        }

        [Server]
        public override void Cancel()
        {
            execute = false;
            aiMovement.StopMove();
        }

        [Server]
        public override bool CanExecute()
        {
            return Mathf.Approximately(aggravate.AggravateValue, 0f);
        }

        [Server]
        public override ActionId GetActionId()
        {
            return ActionId.Patrol;
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
            memory = self.GetComponent<Memory>();
            this.self = self.transform;
            return this;
        }
    }
}
