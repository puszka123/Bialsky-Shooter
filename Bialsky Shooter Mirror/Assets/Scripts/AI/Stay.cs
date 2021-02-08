using BialskyShooter.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    public class Stay : MonoBehaviour, IAction
    {
        [Inject] Aggravate aggravate = null;
        [Inject] AIMovement aiMovement = null;

        bool execute = false;

        void Update()
        {
            if (!execute) return;
            aiMovement.StopMove();
        }

        public void Cancel()
        {
            execute = false;
        }

        public bool CanExecute()
        {
            return Mathf.Approximately(aggravate.AggravateValue, 0f);
        }

        public void Execute()
        {
            execute = true;
        }

        public ActionId GetActionId()
        {
            return ActionId.Stay;
        }
    }
}