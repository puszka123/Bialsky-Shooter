using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ResourcesModule
{
    [Serializable]
    public class Resource
    {
        public float Amount;
        [HideInInspector] public ResourceType ResourceType { get; set; }
        public string DisplayName;

        public Resource Extract(float amount)
        {
            float extractAmount = Mathf.Min(Amount, amount);
            Amount -= extractAmount;
            return new Resource
            {
                Amount = extractAmount,
                ResourceType = ResourceType,
                DisplayName = DisplayName,
            };
        }
    }
}
