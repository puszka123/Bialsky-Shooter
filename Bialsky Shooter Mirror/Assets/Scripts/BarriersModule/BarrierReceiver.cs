using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.BarriersModule
{
    public class BarrierReceiver : NetworkBehaviour
    {
        public IList<Barrier> ActiveBarriers { get; private set; } = new List<Barrier>();

        [Server]
        public void Receive(Barrier barrier)
        {
            ActiveBarriers.Add(barrier);
        }
    }
}
