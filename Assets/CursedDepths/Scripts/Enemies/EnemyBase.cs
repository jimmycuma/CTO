using UnityEngine;
using CursedDepths.Combat;
using CursedDepths.Core;

namespace CursedDepths.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealth = 30;
        [SerializeField] protected int damage = 5;
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected int currencyReward = 3;

        protected int currentHealth;
        protected Rigidbody2D body;
        protected Transform target;

        public static int ActiveCount { get; private set; }

        protected virtual void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            ActiveCount++;
        }

        private void OnDisable()
        {
            ActiveCount = Mathf.Max(0, ActiveCount - 1);
        }

        public void Initialize(Transform targetTransform)
        {
            target = targetTransform;
        }

        protected virtual void Update()
        {
            if (target == null)
            {
                return;
            }

            TickMovement();
        }

        protected virtual void TickMovement()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            body.velocity = direction * moveSpeed;
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void ApplyHit(Vector2 direction)
        {
            body.AddForce(direction * 1.5f, ForceMode2D.Impulse);
        }

        protected virtual void Die()
        {
            if (GameSession.Instance != null)
            {
                GameSession.Instance.RegisterEnemyKill(currencyReward);
            }

            Destroy(gameObject);
        }

        protected void DealContactDamage()
        {
            if (target != null && target.TryGetComponent<Player.PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
