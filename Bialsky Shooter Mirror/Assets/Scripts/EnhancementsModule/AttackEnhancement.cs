using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EnhancementsModule
{
    [Serializable]
    public class AttackEnhancement
    {
        public float value;
        public float percentageValue;
        public AttackType attackType;
        [HideInInspector] public bool used;

        public AttackEnhancement Create()
        {
            return new AttackEnhancement
            {
                value = value,
                percentageValue = percentageValue,
                attackType = attackType,
                used = used,
            };
        }
    }
}
