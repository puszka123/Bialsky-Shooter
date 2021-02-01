using BialskyShooter.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    public class Aggravate : MonoBehaviour
    {
        [SerializeField] float aggravateValue = 0f;
        [SerializeField] float aggravateRange = 5f;
        [Inject] Targeter targeter = null;
        public GameObject NearbyTarget { get; private set; }

        public float AggravateValue { get { return aggravateValue; } }

        private void Update()
        {
            NearbyTarget = targeter.GetClosestTarget(aggravateRange);
            if (NearbyTarget == null)
            {
                aggravateValue = 0f;
                return;
            }
            var distanceToTarget = Vector3.Distance(transform.position, NearbyTarget.transform.position);
            aggravateValue = 1f - Mathf.Clamp(distanceToTarget / aggravateRange, 0f, 1f);
        }
    }
}
