using UnityEngine;
using CursedDepths.Core;
using CursedDepths.Combat;

namespace CursedDepths.Enemies
{
    public class BossEnemy : EnemyBase
    {
        [SerializeField] private string projectilePoolKey = "Projectile";
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private int volleyCount = 5;

        private float attackTimer;

        protected override void Awake()
        {
            base.Awake();
            maxHealth = 240;
            currentHealth = maxHealth;
            damage = 20;
            moveSpeed = 1.4f;
            currencyReward = 50;
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
                FireVolley();
            }
        }

        private void FireVolley()
        {
            for (int i = 0; i < volleyCount; i++)
            {
                float angle = 360f / volleyCount * i;
                Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
                var projectile = GameSession.Instance.PoolManager.Spawn<Projectile>(projectilePoolKey, transform.position, Quaternion.identity);
                if (projectile != null)
                {
                    projectile.Fire(direction, damage, 7f);
                }
            }
        }
    }
}
