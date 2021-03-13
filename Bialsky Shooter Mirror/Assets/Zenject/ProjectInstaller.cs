using BialskyShooter.AI;
using BialskyShooter.AI.Pathfinding;
using BialskyShooter.ItemSystem.UI;
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
        [SerializeField] GameObject graphPrefab = null;
        [SerializeField] GameObject teamManagerPrefab = null;
        [SerializeField] GameObject itemStackerPrefab = null;
        
        public override void InstallBindings()
        {
            Container.Bind<MyNetworkManager>().FromComponentInNewPrefab(networkManagerPrefab).AsSingle().NonLazy();
            Container.Bind<Graph>().FromComponentInNewPrefab(graphPrefab).AsSingle().NonLazy();
            Container.Bind<TeamManager>().FromComponentInNewPrefab(teamManagerPrefab).AsSingle().NonLazy();
            Container.Bind<ItemStacker>().FromComponentInNewPrefab(itemStackerPrefab).AsSingle().NonLazy();
        }
    }
}