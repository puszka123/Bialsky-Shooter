using BialskyShooter.Multiplayer;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class HumanEnemySpawner : NeutralCreaturesSpawner
    {
        [Inject]
        public void Construct(CreatureFactoryBehaviour.HumanEnemyFactory creatureFactory)
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            this.creatureFactory = creatureFactory;
        }
    }
}
