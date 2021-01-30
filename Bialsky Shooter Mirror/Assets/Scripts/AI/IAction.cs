using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public interface IAction
    {
        void Execute();
        void Cancel();
        bool CanExecute();
    }
}
