using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.EnhancementsModule
{
    public class EnhancementReceiver : NetworkBehaviour
    {
        public IList<AttackEnhancement> AttackEnhancements { get; set; } = new List<AttackEnhancement>();

        public void Receive(AttackEnhancement attackEnhancement)
        {
            AttackEnhancements.Add(attackEnhancement);
        }
    }
}
