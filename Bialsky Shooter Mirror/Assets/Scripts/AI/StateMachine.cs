using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(ActionScheduler))]
    public class StateMachine : MonoBehaviour
    {
        [Inject] ActionScheduler actionScheduler;
        [Inject] Patrol patrol;
        [Inject] Fight fight;

        Dictionary<IAction, IAction[]> actionsGraph;

        private void Start()
        {
            actionsGraph = new Dictionary<IAction, IAction[]>
            {
                { patrol, new IAction[] { fight }  },
                { fight, new IAction[] { patrol }  },
            };
            actionScheduler.UpdateCurrentAction(actionsGraph.Keys.First());
        }

        private void Update()
        {
            foreach (var action in actionsGraph[actionScheduler.CurrentAction])
            {
                if (action.CanExecute()) actionScheduler.UpdateCurrentAction(action);
            }
        }

    }
}
