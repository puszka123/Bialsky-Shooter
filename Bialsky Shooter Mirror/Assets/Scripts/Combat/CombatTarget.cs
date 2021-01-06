using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : NetworkBehaviour
    {
        [Inject] Health health = null;

        public Health Health { get { return health; } }
    }
}
