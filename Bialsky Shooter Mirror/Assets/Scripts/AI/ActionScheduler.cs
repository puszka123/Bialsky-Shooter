using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class ActionScheduler : NetworkBehaviour
    {
        IAction currentAction;

        public IAction CurrentAction { get { return currentAction; } }

        [ServerCallback]
        void Update()
        {
            currentAction.Execute();
        }

        [Server]
        public void UpdateCurrentAction(IAction action)
        {
            TryCancelCurrentAction();
            SetCurrentAction(action);
        }

        [Server]
        public void TryCancelCurrentAction()
        {
            if(currentAction != null) currentAction.Cancel();
        }

        [Server]
        void SetCurrentAction(IAction action)
        {
            currentAction = action;
        }
    }
}