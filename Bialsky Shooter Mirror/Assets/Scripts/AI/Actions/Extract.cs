using BialskyShooter.Movement;
using BialskyShooter.ResourcesModule;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    [CreateAssetMenu(fileName = "ExtractCommand", menuName = "ScriptableObjects/AI/Commands/Extract")]
    public class Extract : Action, ICommand
    {
        AIMovement aiMovement = null;
        bool execute;
        Transform self;
        ResourceSource target;
        bool resting;

        public override void Cancel()
        {
            execute = false;
            target = null;
        }

        public override bool CanExecute()
        {
            return target != null;
        }

        public bool Completed()
        {
            return false;
        }

        public override void Execute(MonoBehaviour executor)
        {
            execute = true;
            if (Vector3.Distance(self.transform.position, target.transform.position) > 4f)
            {
                aiMovement.Move(target.transform.position);
            }
            else if(!resting)
            {
                target.Extract(5f, self.GetComponent<NetworkIdentity>());
                aiMovement.StopMove();
                executor.StartCoroutine(Rest());
            }
        }

        IEnumerator Rest()
        {
            resting = true;
            yield return new WaitForSeconds(1f);
            resting = false;
        }

        public override bool Executing()
        {
            return execute;
        }

        public override ActionId GetActionId()
        {
            return ActionId.Extract;
        }

        [Server]
        public override IAction SetSelf(GameObject self)
        {
            aiMovement = self.GetComponent<AIMovement>();
            this.self = self.transform;
            return this;
        }

        public void SetTarget(dynamic target)
        {
            this.target = target.GetComponent<ResourceSource>();
        }
    }
}
