using BialskyShooter.Movement;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "StayAction", menuName ="ScriptableObjects/AI/Actions/Stay")]
    public class Stay : Action, IAction
    {
        Aggravate aggravate = null;
        AIMovement aiMovement = null;

        bool execute = false;

        [Server]
        public override void Cancel()
        {
            execute = false;
        }

        [Server]
        public override bool CanExecute()
        {
            return Mathf.Approximately(aggravate.AggravateValue, 0f);
        }

        [Server]
        public override void Execute(MonoBehaviour executor)
        {
            execute = true;
            aiMovement.StopMove();
        }

        [Server]
        public override ActionId GetActionId()
        {
            return ActionId.Stay;
        }

        [Server]
        public override bool Executing()
        {
            return execute;
        }

        [Server]
        public override IAction SetSelf(GameObject self)
        {
            aiMovement = self.GetComponent<AIMovement>();
            aggravate = self.GetComponent<Aggravate>();
            return this;
        }
    }
}