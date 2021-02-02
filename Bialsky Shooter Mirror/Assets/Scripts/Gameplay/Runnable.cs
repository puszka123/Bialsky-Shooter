using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Gameplay
{
    public class Runnable : NetworkBehaviour
    {
        public IList<IRunnable> GetRunnables()
        {
            return GetComponents<IRunnable>();
        }
    }
}
