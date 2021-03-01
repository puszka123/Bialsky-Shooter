using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.Combat
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] float speed = 150f; //debug
        float damage;
        NetworkIdentity user;
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Init(NetworkIdentity owner, float damage)
        {
            this.user = owner;
            this.damage = damage;
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == user.gameObject
                || !collision.gameObject.TryGetComponent(out CombatTarget target)
                || target.Health.IsDefeated) return;
            target.Health.TakeDamage(user, damage);
            NetworkServer.Destroy(gameObject);
        }

    }
}
