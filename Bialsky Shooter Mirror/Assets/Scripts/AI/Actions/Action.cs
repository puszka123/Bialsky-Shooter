using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public abstract class Action : ScriptableObject, IAction
    {
        public abstract void Cancel();

        public abstract bool CanExecute();

        public abstract void Execute(MonoBehaviour executor);

        public abstract bool Executing();

        public abstract ActionId GetActionId();

        public abstract IAction SetSelf(GameObject self);
    }
}
