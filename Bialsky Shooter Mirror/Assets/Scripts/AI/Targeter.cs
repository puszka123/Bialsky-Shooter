using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    [RequireComponent(typeof(TeamMember))]
    public class Targeter : MonoBehaviour
    {
        [Inject] TeamMember teamMember = null;
        [Inject] TeamManager teamManager = null;
        public GameObject GetClosestTarget(float inRange)
        {
            var closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;
            foreach (var enemy in teamManager.GetAllEnemies(teamMember.TeamId))
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if(distance <= inRange && distance < closestDistance)
                {
                    closestEnemy = enemy.gameObject;
                }
            }
            return closestEnemy;
        }
    }
}
