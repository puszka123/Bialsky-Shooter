using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.BarriersModule
{
    [Serializable]
    public class Barrier
    {
        public float totalAbsorption;
        public float absorptionPercentage;
        [HideInInspector] public float absorptionPercentageValue; 
        public float duration;
        public BarrierType type;

        public Barrier Create()
        {
            return new Barrier
            {
                totalAbsorption = totalAbsorption,
                absorptionPercentage = absorptionPercentage,
                duration = duration,
                type = type,
            };
        }
    }
}
