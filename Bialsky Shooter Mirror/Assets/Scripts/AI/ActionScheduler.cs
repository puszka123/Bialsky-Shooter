using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public IAction CurrentAction { get { return currentAction; } }

        void Update()
        {
            currentAction.Execute();
        }

        public void UpdateCurrentAction(IAction action)
        {
            TryToCancelCurrentAction();
            SetCurrentAction(action);
        }

        void TryToCancelCurrentAction()
        {
            if(currentAction != null) currentAction.Cancel();
        }

        void SetCurrentAction(IAction action)
        {
            currentAction = action;
        }
    }
}