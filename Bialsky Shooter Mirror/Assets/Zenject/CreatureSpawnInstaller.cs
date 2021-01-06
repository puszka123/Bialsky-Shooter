using BialskyShooter.ResourcesModule;
using UnityEngine;
using Zenject;

public class CreatureSpawnInstaller : MonoInstaller
{
    [Inject] Vector3 position = default;
    [Inject] Quaternion rotation = default;
    public override void InstallBindings()
    {
        Container.BindInstance(position).WhenInjectedInto<CreatureFactoryBehaviour>();
        Container.BindInstance(rotation).WhenInjectedInto<CreatureFactoryBehaviour>();
    }
}