using UnityEngine;
using CursedDepths.Core;
using CursedDepths.Enemies;

namespace CursedDepths.Combat
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : Poolable
    {
        [SerializeField] private int damage = 5;
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private LayerMask hitMask;

        private Rigidbody2D body;
        private float lifeTimer;
        private Vector2 direction;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        public void Fire(Vector2 dir, int newDamage, float newSpeed)
        {
            direction = dir.normalized;
            damage = newDamage;
            speed = newSpeed;
            lifeTimer = lifetime;
            body.velocity = direction * speed;
        }

        public override void OnSpawned()
        {
            lifeTimer = lifetime;
        }

        private void Update()
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((hitMask.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }

            if (other.TryGetComponent<EnemyBase>(out var enemy))
            {
                enemy.ApplyHit(direction);
            }

            Despawn();
        }

        private void Despawn()
        {
            if (GameSession.Instance != null)
            {
                GameSession.Instance.PoolManager.Despawn(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
