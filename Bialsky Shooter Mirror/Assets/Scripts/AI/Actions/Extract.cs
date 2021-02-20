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
        ResourceHolder resourceHolder;
        bool extracted;
        bool extracting;
        ResourceDestination resourceDestination;

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
            extracted = resourceHolder.IsHolding;
            if (!extracted && !extracting && Vector3.Distance(self.transform.position, target.transform.position) > 2f)
            {
                aiMovement.Move(target.transform.position);
            }
            else if (!extracted && !extracting)
            {
                target.Extract(resourceHolder.Capacity, self.GetComponent<NetworkIdentity>());
                aiMovement.StopMove();
                executor.StartCoroutine(Extracting());
            }
            else if(extracted && !extracting)
            {
                if (resourceDestination == null) resourceDestination = GetNearestResourceDestination();
                if (resourceDestination == null) return;
                if (Vector3.Distance(self.transform.position, resourceDestination.transform.position) > 2f)
                {
                    aiMovement.Move(resourceDestination.transform.position);
                }
                else
                {
                    aiMovement.StopMove();
                    resourceDestination.GainResource(resourceHolder.GiveBackResource());
                }
            }
        }

        private ResourceDestination GetNearestResourceDestination()
        {
            float nearestResourceDestinationDistance = Mathf.Infinity;
            ResourceDestination nearestResourceDestination = null;
            foreach (var item in FindObjectsOfType<ResourceDestination>())
            {
                if (item.GetComponent<TeamMember>().TeamId != self.GetComponent<TeamMember>().TeamId) continue;
                if(Vector3.Distance(self.transform.position, item.transform.position) < nearestResourceDestinationDistance)
                {
                    nearestResourceDestinationDistance = Vector3.Distance(self.transform.position, item.transform.position);
                    nearestResourceDestination = item;
                }
            }
            return nearestResourceDestination;
        }

        IEnumerator Extracting()
        {
            extracting = true;
            yield return new WaitForSeconds(1f);
            extracting = false;
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
            resourceHolder = self.GetComponent<ResourceHolder>();
            this.self = self.transform;
            return this;
        }

        public void SetTarget(dynamic target)
        {
            this.target = target.GetComponent<ResourceSource>();
        }
    }
}
