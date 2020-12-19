using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.ResourcesModule
{
    public class CreatureFactoryBehaviour : MonoBehaviour
    {
        [Inject]
        public void Construct(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            print(gameObject.name + " " + position);
        }

        public class CreatureFactory : PlaceholderFactory<Vector3, Quaternion, CreatureFactoryBehaviour>
        {

        }
    }
}
