using BialskyShooter.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    public class Move : MonoBehaviour, ICommand
    {
        [Inject] AIMovement aiMovement = null;
        bool execute;
        Vector3 target;

        void Update()
        {
            if (!execute) return;
            aiMovement.Move(target);

        }
        public void Cancel()
        {
            execute = false;
            target = Vector3.zero;
        }

        public bool CanExecute()
        {
            return target != Vector3.zero;
        }

        public void Execute()
        {
            execute = true;
        }

        public ActionId GetActionId()
        {
            return ActionId.Move;
        }

        public bool Completed()
        {
            return Vector3.Distance(transform.position, target) <= 2f;
        }

        public void SetTarget(dynamic target)
        {
            this.target = (Vector3)target;
        }
    }
}
