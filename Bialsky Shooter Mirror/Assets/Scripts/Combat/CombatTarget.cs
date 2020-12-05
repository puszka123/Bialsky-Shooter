using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : NetworkBehaviour
    {
        Health health;

        public Health Health { get { return health; } }

        #region Server

        public override void OnStartServer()
        {
            health = GetComponent<Health>();
        }

        #endregion
    }
}
