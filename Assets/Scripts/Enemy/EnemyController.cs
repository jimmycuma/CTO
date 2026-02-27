using UnityEngine;
using CursedDepths.Combat;

namespace CursedDepths.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public enum EnemyType
        {
            Slime,
            Skeleton,
            Ghost,
            Boss
        }

        [Header("Enemy Stats")]
        public EnemyType enemyType;
        public int maxHealth = 50;
        public int damage = 10;
        public float moveSpeed = 2f;
        public int goldReward = 10;
        public float detectionRange = 5f;
        public float attackRange = 1f;
        public float attackCooldown = 1f;

        [Header("References")]
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Transform player;

        [Header("Drops")]
        public GameObject[] possibleDrops;
        public float dropChance = 0.3f;

        private int currentHealth;
        private bool isDead = false;
        private bool canAttack = true;
        private bool isChasing = false;

        private void Start()
        {
            currentHealth = maxHealth;
            
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player")?.transform;
            }

            SetupEnemyType();
        }

        private void SetupEnemyType()
        {
            switch (enemyType)
            {
                case EnemyType.Slime:
                    maxHealth = 30;
                    damage = 5;
                    moveSpeed = 1.5f;
                    goldReward = 5;
                    break;
                case EnemyType.Skeleton:
                    maxHealth = 50;
                    damage = 10;
                    moveSpeed = 2f;
                    goldReward = 15;
                    break;
                case EnemyType.Ghost:
                    maxHealth = 35;
                    damage = 15;
                    moveSpeed = 2.5f;
                    goldReward = 20;
                    break;
                case EnemyType.Boss:
                    maxHealth = 200;
                    damage = 25;
                    moveSpeed = 1f;
                    goldReward = 100;
                    break;
            }
            
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (isDead) return;

            CheckForPlayer();
            HandleMovement();
            HandleAttack();
        }

        private void CheckForPlayer()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance <= detectionRange)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }

            animator.SetBool("IsChasing", isChasing);
        }

        private void HandleMovement()
        {
            if (!isChasing || player == null) return;

            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            
            FlipSprite(direction.x);
        }

        private void HandleAttack()
        {
            if (!canAttack || player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance <= attackRange)
            {
                Attack();
            }
        }

        private void Attack()
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            
            animator.SetTrigger("Hit");
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            animator.SetTrigger("Death");
            
            GetComponent<Collider2D>().enabled = false;
            
            Core.EventManager.Instance?.EnemyKilled(goldReward);
            GameManager.Instance?.AddGold(goldReward);
            
            DropItems();
            
            Destroy(gameObject, 1f);
        }

        private void DropItems()
        {
            if (Random.value <= dropChance && possibleDrops.Length > 0)
            {
                GameObject drop = Instantiate(possibleDrops[Random.Range(0, possibleDrops.Length)]);
                drop.transform.position = transform.position;
            }
        }

        private void FlipSprite(float direction)
        {
            if (direction < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
