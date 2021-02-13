using BialskyShooter.AI;
using BialskyShooter.AI.Pathfinding;
using BialskyShooter.ClassSystem;
using BialskyShooter.Combat;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.InventoryModule;
using BialskyShooter.Movement;
using BialskyShooter.Multiplayer;
using BialskyShooter.ResourcesModule;
using BialskyShooter.SkillSystem;
using UnityEngine;
using Zenject;

namespace BialskyShooter.Zenject
{
    public class ComponentInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(convention => convention
                                    .AllClasses()
                                    .InNamespace("BialskyShooter")
                                    .DerivingFrom<MonoBehaviour>()
                                    .Where(e => !e.IsAssignableFrom(typeof(Graph)) && !e.IsAbstract))
                .FromComponentOn(gameObject).AsSingle();
            Container.Unbind<TeamManager>();
            Container.Unbind<MyNetworkManager>();
            Container.Bind<Rigidbody>().FromComponentSibling();
            Container.Bind<MovementSystem>().AsSingle();
        }
    }
}