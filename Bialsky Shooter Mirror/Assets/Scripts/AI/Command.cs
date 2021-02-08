using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class Command
    {
        [Serializable]
        public class CommandArgs
        {
            public ActionId ActionId { get; private set; }
            public object TargetValue;

            public CommandArgs() { }

            public CommandArgs(ActionId actionId, object targetValue)
            {
                ActionId = actionId;
                TargetValue = targetValue;
            }
        }

        public ICommand CommandToExecute { get; private set; }
        public dynamic Target { get; private set; }

        public bool Completed 
        { 
            get 
            {
                if (CommandToExecute == null) return true;
                if (Started && CommandToExecute.Completed()) return true;
                return false;
            } 
        }
        public bool Started { get; private set; }

        public Command(ICommand command, dynamic target)
        {
            CommandToExecute = command;
            Target = target;
        }

        public void Start()
        {
            if (Started) return;
            Started = true;
            SetTarget();
        }

        public void Stop()
        {
            CommandToExecute.Cancel();
        }

        public void SetTarget()
        {
            CommandToExecute.SetTarget(Target);
        }

    }
}
