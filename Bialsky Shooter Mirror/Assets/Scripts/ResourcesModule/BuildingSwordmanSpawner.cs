using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class BuildingSwordmanSpawner : BuildingSpawner
    {
        [Inject]
        public void Construct(CreatureFactoryBehaviour.SwordmanFactory creatureFactory)
        {
            if (NetworkServer.active) NetworkServer.Spawn(gameObject);
            this.creatureFactory = creatureFactory;
        }
    }
}
