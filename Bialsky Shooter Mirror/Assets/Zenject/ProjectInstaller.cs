using BialskyShooter.AI;
using BialskyShooter.Multiplayer;
using BialskyShooter.ResourcesModule;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Zenject
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        [SerializeField] GameObject networkManagerPrefab = null;
        
        public override void InstallBindings()
        {
            Container.Bind<MyNetworkManager>().FromComponentInNewPrefab(networkManagerPrefab).AsSingle().NonLazy();
        }
    }
}