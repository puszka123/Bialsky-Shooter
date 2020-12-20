using BialskyShooter.ResourcesModule;
using UnityEngine;
using Zenject;

public class CreatureSpawnInstaller : MonoInstaller
{
    [Inject] Vector3 position;
    [Inject] Quaternion rotation;
    public override void InstallBindings()
    {
        Container.BindInstance(position).WhenInjectedInto<CreatureFactoryBehaviour>();
        Container.BindInstance(rotation).WhenInjectedInto<CreatureFactoryBehaviour>();
    }
}