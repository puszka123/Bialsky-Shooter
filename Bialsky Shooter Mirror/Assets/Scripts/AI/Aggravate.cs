using BialskyShooter.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public class Aggravate : MonoBehaviour
    {
        [SerializeField] float aggravateValue = 0f;
        [SerializeField] float aggravateRange = 5f;
        CombatTarget nearbyCombatTarget;

        public float AggravateValue { get { return aggravateValue; } }

        private void Update()
        {
            GetNearbyCombatTarget();
            var distanceToTarget = Vector3.Distance(transform.position, nearbyCombatTarget.transform.position);
            aggravateValue = 1f - Mathf.Clamp(distanceToTarget / aggravateRange, 0f, 1f);
        }

        private void GetNearbyCombatTarget()
        {
            if (nearbyCombatTarget == null)
            {
                nearbyCombatTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatTarget>();
            }
        }
    }
}
