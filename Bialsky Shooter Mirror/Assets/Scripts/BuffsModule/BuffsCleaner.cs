using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.BuffsModule
{
    [RequireComponent(typeof(BuffsReceiver))]
    public class BuffsCleaner : NetworkBehaviour
    {
        [Inject] BuffsReceiver buffsReceiver;
        List<Buff> buffsToRemove = new List<Buff>();

        [ServerCallback]
        private void Update()
        {
            foreach (var buff in buffsReceiver.ActiveBuffs)
            {
                buff.duration -= Time.fixedDeltaTime;
                if (buff.duration <= 0f) buffsToRemove.Add(buff);
            }
            foreach (var buffToRemove in buffsToRemove)
            {
                buffsReceiver.RemoveBuff(buffToRemove);
            }
            buffsToRemove.Clear();
        }
    }
}
