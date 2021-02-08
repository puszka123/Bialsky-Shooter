using BialskyShooter.Movement;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(AIMovement))]
    [RequireComponent(typeof(Aggravate))]
    public class Patrol : NetworkBehaviour, IAction
    {
        [SerializeField] float patrolTime = 5f;
        [SerializeField] float patrolRange = 5f;
        [SerializeField] int newPatrolPlaceAttempts = 30;
        [SerializeField] float distanceToPatrolPosition = 2f;
        [Inject] AIMovement aiMovement = null;
        [Inject] Aggravate aggravate = null;
        bool patrol = false;
        bool movingToNewPosition = false;
        Vector3 patrolPosition;
        bool execute = false;

        public Vector3 SpawnerPosition { get; set; }
        public float SpawnRange { get; set; }

        [ServerCallback]
        void Update()
        {
            if (!execute) return;

            if(movingToNewPosition)
            {
                aiMovement.Move(patrolPosition);
                movingToNewPosition = Vector3.Distance(transform.position, patrolPosition) > distanceToPatrolPosition;
            }
            else if(!patrol)
            {
                StartCoroutine(PatrolArea());
            }
        }

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
                if (Vector3.Distance(SpawnerPosition, patrolPosition) <= SpawnRange)
                {
                    movingToNewPosition = true;
                    break;
                }
            }
            if(!movingToNewPosition)
            {
                patrolPosition = SpawnerPosition;
                movingToNewPosition = true;
            }
        }

        [Server]
        private Vector3 GetPatrolPosition()
        {
            var seed = Random.insideUnitCircle * patrolRange;
            var newPatrolPosition = new Vector3(transform.position.x + seed.x, transform.position.y, transform.position.z + seed.y);
            return newPatrolPosition;
        }

        [Server]
        public void Execute()
        {
            execute = true;
        }

        [Server]
        public void Cancel()
        {
            execute = false;
            aiMovement.StopMove();
        }

        [Server]
        public bool CanExecute()
        {
            return Mathf.Approximately(aggravate.AggravateValue, 0f);
        }

        public ActionId GetActionId()
        {
            return ActionId.Patrol;
        }
    }
}
