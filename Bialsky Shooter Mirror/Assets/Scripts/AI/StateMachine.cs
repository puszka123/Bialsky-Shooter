using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Mirror;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(ActionScheduler))]
    public class StateMachine : NetworkBehaviour
    {
        [Inject] ActionScheduler actionScheduler = null;
        [Inject] Patrol patrol = null;
        [Inject] Fight fight = null;
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
            actionsGraph = new Dictionary<IAction, IAction[]>
            {
                { patrol, new IAction[] { fight }  },
                { fight, new IAction[] { patrol }  },
            };
            actionScheduler.UpdateCurrentAction(actionsGraph.Keys.First());
        }

        [ServerCallback]
        private void Update()
        {
            if(commandReceiver != null)
            {
                commandReceiver.ExecuteCommand();
                if (commandReceiver.Executing) return;
            }

            foreach (var action in actionsGraph[actionScheduler.CurrentAction])
            {
                if (action.CanExecute()) actionScheduler.UpdateCurrentAction(action);
            }
        }

    }
}
