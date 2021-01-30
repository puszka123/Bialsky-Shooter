using BialskyShooter.Multiplayer;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class HumanEnemySpawner : EnemySpawner
    {
        [Inject]
        public void Construct(CreatureFactoryBehaviour.HumanEnemyFactory creatureFactory, MyNetworkManager networkManager)
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            this.creatureFactory = creatureFactory;
            this.networkManager = networkManager;
        }
    }
}
