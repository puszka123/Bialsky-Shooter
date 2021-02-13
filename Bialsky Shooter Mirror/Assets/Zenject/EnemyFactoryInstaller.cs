using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using BialskyShooter.ResourcesModule;
using Mirror;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Zenject
{
    [CreateAssetMenu(fileName = "EnemyFactoryInstaller", menuName = "Installers/EnemyFactoryInstaller")]
    public class EnemyFactoryInstaller : ScriptableObjectInstaller<EnemyFactoryInstaller>
    {
        [SerializeField] GameObject humanEnemySpawnerPrefab = null;
        [SerializeField] GameObject boxEnemySpawnerPrefab = null;
        [SerializeField] GameObject playerSpawnerPrefab = null;
        [SerializeField] GameObject humanEnemyPrefab = null;
        [SerializeField] GameObject boxEnemyPrefab = null;
        [SerializeField] GameObject playerPrefab = null;
        public override void InstallBindings()
        {
            Container.Bind<Vector3>().AsSingle();
            Container.Bind<Quaternion>().AsSingle();
            Container.Bind<float>().AsSingle();
            if (NetworkServer.active)
            {
                Container.BindFactory<Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.PlayerFactory>()
                    .FromSubContainerResolve().ByNewContextPrefab<CreatureSpawnInstaller>(playerPrefab);
                Container.BindFactory<Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.HumanEnemyFactory>()
                    .FromSubContainerResolve().ByNewContextPrefab<CreatureSpawnInstaller>(humanEnemyPrefab);
                Container.BindFactory<Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.BoxEnemyFactory>()
                    .FromSubContainerResolve().ByNewContextPrefab<CreatureSpawnInstaller>(boxEnemyPrefab);
                Container.Bind<PlayerSpawner>().FromComponentInNewPrefab(playerSpawnerPrefab).AsTransient().NonLazy();
                Container.Bind<HumanEnemySpawner>().FromComponentInNewPrefab(humanEnemySpawnerPrefab).AsTransient().NonLazy();
                Container.Bind<BoxEnemySpawner>().FromComponentInNewPrefab(boxEnemySpawnerPrefab).AsTransient().NonLazy();
            }
        }
    }
}