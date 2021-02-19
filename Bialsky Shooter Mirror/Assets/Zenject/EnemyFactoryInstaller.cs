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
        [SerializeField] GameObject playerSpawnerPrefab = null;
        public override void InstallBindings()
        {
            Container.Bind<Vector3>().AsSingle();
            Container.Bind<Quaternion>().AsSingle();
            Container.Bind<float>().AsSingle();

            Container.BindFactory<GameObject, Vector3, Quaternion, CreatureFactoryBehaviour, CreatureFactoryBehaviour.CreatureFactory>();

            if (NetworkServer.active)
            {
                Container.Bind<PlayerSpawner>().FromComponentInNewPrefab(playerSpawnerPrefab).AsTransient().NonLazy();
            }
        }
    }
}