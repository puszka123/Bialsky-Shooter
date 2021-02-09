using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public interface IAction
    {
        void Execute(MonoBehaviour executor);
        void Cancel();
        bool CanExecute();
        bool Executing();
        ActionId GetActionId();
        IAction SetSelf(GameObject self);
    }
}
