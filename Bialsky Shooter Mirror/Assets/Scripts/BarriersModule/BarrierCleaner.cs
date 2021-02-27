using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.BarriersModule
{
    [RequireComponent(typeof(BarrierReceiver))]
    public class BarrierCleaner : NetworkBehaviour
    {
        [Inject] BarrierReceiver barrierReceiver;
        IList<Barrier> barriersToRemove = new List<Barrier>();

        private void Update()
        {
            UpdateActiveBarriers();
            RemoveExpiredBarriers();
        }

        private void UpdateActiveBarriers()
        {
            foreach (var barrier in barrierReceiver.ActiveBarriers)
            {
                barrier.duration -= Time.fixedDeltaTime;
                if (barrier.duration <= 0f) barriersToRemove.Add(barrier);
            }
        }

        private void RemoveExpiredBarriers()
        {
            foreach (var barrierToRemove in barriersToRemove)
            {
                barrierReceiver.ActiveBarriers.Remove(barrierToRemove);
            }
            barriersToRemove.Clear();
        }
    }
}