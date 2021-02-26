using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.EnhancementsModule
{
    [RequireComponent(typeof(EnhancementReceiver))]
    public class EnhancementSender : NetworkBehaviour
    {
        [Inject] EnhancementReceiver enhancementReceiver;

        public IEnumerable<AttackEnhancement> Get(AttackType attackType)
        {
            var enhancements = new List<AttackEnhancement>();
            foreach (var enhancement in enhancementReceiver.AttackEnhancements)
            {
                if (enhancement.attackType == attackType) enhancements.Add(enhancement);
            }
            foreach (var enhancement in enhancements)
            {
                enhancementReceiver.AttackEnhancements.Remove(enhancement);
            }
            return enhancements;
        }
    }
}
