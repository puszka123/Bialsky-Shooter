using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BialskyShooter.AI.Command;

namespace BialskyShooter.AI
{
    public interface ICommand : IAction
    {
        bool Completed();
        void SetTarget(dynamic target);
    }
}
