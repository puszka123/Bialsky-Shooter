using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using BialskyShooter.ResourcesModule;
using Mirror;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Zenject
{
    [CreateAssetMenu(fileName = "DefaultInstaller", menuName = "Installers/DefaultInstaller")]
    public class ComponentInstaller : ScriptableObjectInstaller<ComponentInstaller>
    {
        [SerializeField] GameObject enemySpawnerPrefab;
        [SerializeField] GameObject humanEnemyPrefab;
        public override void InstallBindings()
        {
            Container.Bind(convention => convention
                                .AllClasses()
                                .InNamespace("BialskyShooter")
                                .DerivingFrom<MonoBehaviour>()).FromComponentSibling();
            Container.Unbind<MyNetworkManager>();
            Container.Bind<Rigidbody>().FromComponentSibling();
            Container.Bind<MovementSystem>().AsSingle();
            Container.Bind<Vector3>().AsSingle();
            Container.Bind<float>().AsSingle();
            Container.Bind<Quaternion>().AsSingle();
            Container.BindFactory<Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.CreatureFactory>()
                .FromComponentInNewPrefab(humanEnemyPrefab);
            Container.Bind<EnemySpawner>().FromComponentInNewPrefab(enemySpawnerPrefab).AsSingle().NonLazy();
        }
    }
}