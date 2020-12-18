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
        [Inject] Health health;

        public Health Health { get { return health; } }
    }
}
