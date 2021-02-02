using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Gameplay
{
    public interface IRunnable
    {
        void Run();
        int Priority();
    }
}
