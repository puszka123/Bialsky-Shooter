using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.BarriersModule
{
    [RequireComponent(typeof(BarrierReceiver))]
    public class BarrierHandler : NetworkBehaviour
    {
        [Inject] BarrierReceiver barrierReceiver;

        public float Absorb(float damage, float value, BarrierType barrierType)
        {
            foreach (var barrier in GetBarriers(barrierType))
            {
                if (barrier.absorptionPercentageValue == 0f)
                {
                    barrier.absorptionPercentageValue = value * (barrier.absorptionPercentage / 100);
                    barrier.totalAbsorption += barrier.absorptionPercentageValue;
                }
                barrier.totalAbsorption -= damage;
                if (barrier.totalAbsorption >= 0f) damage = 0f;
                else damage = -barrier.totalAbsorption;
            }

            return damage;
        }

        IList<Barrier> GetBarriers(BarrierType barrierType)
        {
            var barriers = new List<Barrier>();
            foreach (var barrier in barrierReceiver.ActiveBarriers)
            {
                if (barrier.type == barrierType) barriers.Add(barrier);
            }
            return barriers;
        }
    }
}
