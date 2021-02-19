using BialskyShooter.AI;
using BialskyShooter.Gameplay;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public abstract class Spawner : NetworkBehaviour, IRunnable
    {
        [SerializeField] protected BattleSceneManager.Priority priority = default;
        [SerializeField] protected StateGraph stateGraph = null;
        [SerializeField] protected GameObject spawningCreaturePrefab;
        [Inject] protected CreatureFactoryBehaviour.CreatureFactory creatureFactory;
        public int Priority()
        {
            return (int)priority;
        }

        public abstract void Run();
        protected abstract Vector3 GetSpawnPosition();
    }
}
