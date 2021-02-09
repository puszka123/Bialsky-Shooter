using BialskyShooter.Movement;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "MoveCommand", menuName = "ScriptableObjects/AI/Commands/Move")]
    public class Move : Action, ICommand
    {
        AIMovement aiMovement = null;
        bool execute;
        Vector3 target;
        Transform self;

        [Server]
        public override void Cancel()
        {
            execute = false;
            target = Vector3.zero;
        }

        [Server]
        public override bool CanExecute()
        {
            return target != Vector3.zero;
        }

        [Server]
        public override void Execute(MonoBehaviour executor)
        {
            execute = true;
            aiMovement.Move(target);
        }

        [Server]
        public override ActionId GetActionId()
        {
            return ActionId.Move;
        }

        [Server]
        public bool Completed()
        {
            return Vector3.Distance(self.position, target) <= 2f;
        }

        [Server]
        public override IAction SetSelf(GameObject self)
        {
            aiMovement = self.GetComponent<AIMovement>();
            this.self = self.transform;
            return this;
        }

        [Server]
        public void SetTarget(dynamic target)
        {
            this.target = (Vector3)target;
        }

        [Server]
        public override bool Executing()
        {
            return execute;
        }
    }
}
