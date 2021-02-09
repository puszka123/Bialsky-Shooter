using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class ActionExecutor : MonoBehaviour
    {
        IAction actionToExecute;
        void Update()
        {
            if (actionToExecute == null) return;
            actionToExecute.Execute(this);
        }

        public void Init(IAction action)
        {
            actionToExecute = action;
        }
    }
}
