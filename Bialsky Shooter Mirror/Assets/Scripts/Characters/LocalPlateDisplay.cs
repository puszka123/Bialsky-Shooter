using BialskyShooter.ClassSystem;
using BialskyShooter.Combat;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.CharacterModule
{
    public class LocalPlateDisplay : CreaturePlateDisplay
    {
        private void TryInit()
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    Init(player.GetComponent<Health>(), player.GetComponent<CreatureStats>());
                    break;
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (initialized) return;
            TryInit();
        }
    }
}
