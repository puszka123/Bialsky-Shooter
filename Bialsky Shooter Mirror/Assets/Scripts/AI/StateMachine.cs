using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Mirror;
using System;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(ActionScheduler))]
    public class StateMachine : NetworkBehaviour
    {
        [Inject] ActionScheduler actionScheduler = null;
        [SerializeField] StateGraph stateGraph;
        CommandReceiver commandReceiver;
        Dictionary<IAction, IAction[]> actionsGraph;

        [ServerCallback]
        private void Awake()
        {
            commandReceiver = GetComponent<CommandReceiver>();
        }

        [ServerCallback]
        private void Start()
        {
            if (stateGraph != null)
            {
                InitActionsGraph();
                actionScheduler.UpdateCurrentAction(actionsGraph.Keys.First());
            }
        }

        public void Init(StateGraph stateGraph)
        {
            this.stateGraph = stateGraph;
            InitActionsGraph();
            actionScheduler.UpdateCurrentAction(actionsGraph.Keys.First());
        }

        private void InitActionsGraph()
        {
            actionsGraph = new Dictionary<IAction, IAction[]>();
            foreach (var node in stateGraph.actionNodes)
            {
                var action = GetComponents<IAction>().First(a => a.GetActionId() == node.action);
                var actionConnections = GetComponents<IAction>().Where(a => node.actionConnections.Contains(a.GetActionId()));
                actionsGraph[action] = actionConnections.ToArray();
            }
        }

        [ServerCallback]
        private void Update()
        {
            if(commandReceiver != null)
            {
                HandleCommand();
                if (commandReceiver.Executing) return;
            }

            if (actionsGraph == null) return;
            if(!actionsGraph.ContainsKey(actionScheduler.CurrentAction))
            {
                actionScheduler.UpdateCurrentAction(actionsGraph.Keys.First());
            }
            foreach (var action in actionsGraph[actionScheduler.CurrentAction])
            {
                if (action.CanExecute()) actionScheduler.UpdateCurrentAction(action);
            }
        }

        private void HandleCommand()
        {
            var command = commandReceiver.GetCommandToExecute();
            if (command != null && !commandReceiver.Executing)
            {
                actionScheduler.UpdateCurrentAction(command.CommandToExecute);
                command.Start();
            }
        }
    }
}
