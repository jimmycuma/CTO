using UnityEngine;
using CursedDepths.Combat;
using CursedDepths.Core;

namespace CursedDepths.Enemies
{
    public class RangedEnemy : EnemyBase
    {
        [SerializeField] private string projectilePoolKey = "Projectile";
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private float keepDistance = 3.5f;

        private float attackTimer;

        protected override void TickMovement()
        {
            if (target == null)
            {
                return;
            }

            float distance = Vector2.Distance(transform.position, target.position);
            Vector2 direction = (target.position - transform.position).normalized;
            if (distance > keepDistance)
            {
                body.velocity = direction * moveSpeed;
            }
            else
            {
                body.velocity = -direction * moveSpeed * 0.5f;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (target == null)
            {
                return;
            }

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
                var projectile = GameSession.Instance.PoolManager.Spawn<Projectile>(projectilePoolKey, transform.position, Quaternion.identity);
                if (projectile != null)
                {
                    Vector2 direction = (target.position - transform.position).normalized;
                    projectile.Fire(direction, damage, 8f);
                }
            }
        }
    }
}
