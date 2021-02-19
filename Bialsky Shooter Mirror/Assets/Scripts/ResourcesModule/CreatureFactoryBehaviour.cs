using Mirror;
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
        }

        public class CreatureFactory : PlaceholderFactory<GameObject, Vector3, Quaternion, CreatureFactoryBehaviour>
        {
            public override CreatureFactoryBehaviour Create(GameObject param1, Vector3 param2, Quaternion param3)
            {
                var instance = Instantiate(param1);
                var creatureFactoryBehaviour = instance.GetComponent<CreatureFactoryBehaviour>();
                creatureFactoryBehaviour.Construct(param2, param3);
                return creatureFactoryBehaviour;
            }
        }

        //public class PlayerFactory : CreatureFactory
        //{

        //}

        //public class HumanEnemyFactory : CreatureFactory
        //{
            
        //}

        //public class BoxEnemyFactory : CreatureFactory
        //{ 

        //}

        //public class SwordmanFactory : CreatureFactory
        //{

        //}

        //public class GoldMinerFactory : CreatureFactory
        //{
        //    override 
        //}
    }
}
