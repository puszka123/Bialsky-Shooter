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
        [SerializeField] GameObject enemySpawnerPrefab;
        [SerializeField] GameObject humanEnemyPrefab;
        public override void InstallBindings()
        {
            Container.Bind<Vector3>().AsSingle();
            Container.Bind<Quaternion>().AsSingle();
            Container.Bind<float>().AsSingle();
            Container.BindFactory<Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.CreatureFactory>()
                .FromSubContainerResolve().ByNewContextPrefab<CreatureSpawnInstaller>(humanEnemyPrefab);
            Container.Bind<EnemySpawner>().FromComponentInNewPrefab(enemySpawnerPrefab).AsSingle().NonLazy();
        }
    }
}