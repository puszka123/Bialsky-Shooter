using BialskyShooter.AI;
using BialskyShooter.ClassSystem;
using BialskyShooter.Combat;
using BialskyShooter.EquipmentSystem;
using BialskyShooter.InventoryModule;
using BialskyShooter.Movement;
using BialskyShooter.SkillSystem;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "DefaultInstaller", menuName = "Installers/DefaultInstaller")]
public class DefaultInstaller : ScriptableObjectInstaller<DefaultInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<CreatureStats>().FromComponentsSibling();
        Container.Bind<SkillsDisplay>().FromComponentsSibling();
        Container.Bind<EnemySight>().FromComponentsSibling();
        Container.Bind<AIMovement>().FromComponentsSibling();
        Container.Bind<SkillUser>().FromComponentsSibling();
        Container.Bind<Experience>().FromComponentsSibling();
        Container.Bind<PlayerMovement>().FromComponentsSibling();
        Container.Bind<Inventory>().FromComponentsSibling();
        Container.Bind<Equipment>().FromComponentsSibling();
        Container.Bind<Movement>().FromComponentsSibling();
        Container.Bind<CollisionDetection>().FromComponentsSibling();
        Container.Bind<Rigidbody>().FromComponentsSibling();
        Container.Bind<WeaponUser>().FromComponentsSibling();
        Container.Bind<SkillsBook>().FromComponentsSibling();
        Container.Bind<Health>().FromComponentsSibling();
        Container.Bind<MovementSystem>().AsSingle();
    }
}