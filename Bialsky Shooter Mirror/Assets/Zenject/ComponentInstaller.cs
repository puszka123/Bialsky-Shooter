using BialskyShooter.AI;
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
                                    .Where(e => !e.IsAssignableFrom(typeof(WeaponController))))
                .FromComponentOn(gameObject).AsSingle();
            Container.Bind<Rigidbody>().FromComponentSibling();
            Container.Bind<MovementSystem>().AsSingle();
        }
    }
}