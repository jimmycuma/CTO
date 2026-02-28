using UnityEngine;

namespace CursedDepths.Enemies
{
    public class MeleeEnemy : EnemyBase
    {
        [SerializeField] private float attackCooldown = 1.2f;
        private float attackTimer;

        protected override void Update()
        {
            base.Update();
            if (target == null)
            {
                return;
            }

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f && Vector2.Distance(transform.position, target.position) < 1.1f)
            {
                attackTimer = attackCooldown;
                DealContactDamage();
            }
        }
    }
}
