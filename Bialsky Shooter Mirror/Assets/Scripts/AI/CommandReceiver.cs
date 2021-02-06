using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BialskyShooter.AI.Command;

namespace BialskyShooter.AI
{
    public class CommandReceiver : NetworkBehaviour
    {
        public IList<Command> CommandsToExecute { get; set; }

        public bool Executing 
        { 
            get
            {
                foreach (var cmd in CommandsToExecute)
                {
                    if (!cmd.Completed && cmd.Started) return true;
                }
                return false;
            }
        }

        [ServerCallback]
        private void Awake()
        {
            CommandsToExecute = new List<Command>();
        }

        [Server]
        public void ReceiveCommand(CommandArgs commandArgs)
        {
            AddCommand(commandArgs, GetComponents<ICommand>());
        }

        public void AddCommand(CommandArgs commandArgs, IList<ICommand> commands)
        {
            var command = commands.First(c => c.GetCommandId() == commandArgs.CommandId);
            CommandsToExecute.Add(new Command(command, commandArgs.TargetValue));
        }

        public void ClearCommands()
        {
            CommandsToExecute = new List<Command>();
        }

        public Command GetCommandToExecute()
        {
            var cmd = CommandsToExecute.FirstOrDefault();
            return cmd != null && !cmd.Started ? cmd : null;
        }

        public void ExecuteCommand()
        {
            var cmd = GetCommandToExecute();
            if(cmd == null)
            {
                return;
            }
            cmd.Start();
        }
    }
}