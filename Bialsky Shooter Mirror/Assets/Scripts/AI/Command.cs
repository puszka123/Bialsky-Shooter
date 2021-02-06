using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class Command
    {
        public enum CommandId
        { 
            Fight,
        }

        [Serializable]
        public class CommandArgs
        {
            public CommandId CommandId { get; private set; }
            public object TargetValue;

            public CommandArgs() { }

            public CommandArgs(CommandId commandId, object targetValue)
            {
                CommandId = commandId;
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
            CommandToExecute.SetTarget(Target);
            CommandToExecute.Execute();
        }
    }
}
