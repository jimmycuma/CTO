using UnityEngine;
using CursedDepths.Core;
using CursedDepths.Combat;
using CursedDepths.Inventory;

namespace CursedDepths.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public LayerMask groundLayer;
        public float groundCheckRadius = 0.2f;

        [Header("Combat")]
        public int baseDamage = 10;
        public float attackRange = 1.5f;
        public Transform attackPoint;
        public LayerMask enemyLayer;
        public float attackCooldown = 0.5f;
        public LayerMask interactableLayer;

        [Header("Health")]
        public int maxHealth = 100;
        public int currentHealth;

        [Header("Components")]
        public Animator animator;
        public Rigidbody2D rb;
        public SpriteRenderer spriteRenderer;

        private bool isGrounded;
        private bool facingRight = true;
        private float lastAttackTime;
        private InventorySystem inventorySystem;

        private void Start()
        {
            currentHealth = maxHealth;
            inventorySystem = GetComponent<InventorySystem>();
            GameManager.Instance.SetPlayer(this);
            EventManager.Instance?.PlayerHealthUpdated(currentHealth, maxHealth);
        }

        private void Update()
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Playing)
                return;

            HandleMovement();
            HandleJump();
            HandleAttack();
            HandleInteraction();
            HandleFacingDirection();
        }

        private void HandleMovement()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
            rb.velocity = movement * moveSpeed;

            animator.SetFloat("Speed", movement.magnitude);
            animator.SetFloat("Horizontal", horizontalInput);
            animator.SetFloat("Vertical", verticalInput);
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            animator.SetBool("IsGrounded", isGrounded);
        }

        private void HandleAttack()
        {
            if (Input.GetButtonDown("Fire") && Time.time >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = Time.time;
            }
        }

        private void PerformAttack()
        {
            animator.SetTrigger("Attack");
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    int totalDamage = baseDamage + inventorySystem.GetAttackBonus();
                    damageable.TakeDamage(totalDamage);
                    SpawnHitEffect(enemy.transform.position);
                }
            }
        }

        private void HandleInteraction()
        {
            if (Input.GetButtonDown("Interact"))
            {
                Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, 1f, interactableLayer);
                
                foreach (Collider2D interactable in interactables)
                {
                    IInteractable inter = interactable.GetComponent<IInteractable>();
                    if (inter != null)
                    {
                        inter.Interact(this);
                    }
                }
            }
        }

        private void HandleFacingDirection()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            
            if (horizontalInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            
            EventManager.Instance?.PlayerHealthChanged(currentHealth);
            EventManager.Instance?.PlayerHealthUpdated(currentHealth, maxHealth);
            
            animator.SetTrigger("Hit");
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            
            EventManager.Instance?.PlayerHealthChanged(currentHealth);
            EventManager.Instance?.PlayerHealthUpdated(currentHealth, maxHealth);
        }

        private void Die()
        {
            animator.SetTrigger("Death");
            EventManager.Instance?.PlayerDeath();
            
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            
            GameManager.Instance.GameOver();
        }

        private void SpawnHitEffect(Vector3 position)
        {
            GameObject hitEffect = new GameObject("HitEffect");
            hitEffect.transform.position = position;
            
            SpriteRenderer sr = hitEffect.AddComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite();
            sr.color = new Color(1f, 0.5f, 0f, 0.7f);
            sr.sortingLayerName = "Foreground";
            sr.sortingOrder = 100;
            
            Destroy(hitEffect, 0.2f);
        }

        private Sprite CreateCircleSprite()
        {
            Texture2D texture = new Texture2D(32, 32);
            Color[] colors = new Color[32 * 32];
            
            for (int i = 0; i < colors.Length; i++)
            {
                int x = i % 32;
                int y = i / 32;
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(15.5f, 15.5f));
                colors[i] = distance <= 15.5f ? Color.white : Color.clear;
            }
            
            texture.SetPixels(colors);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            }
            
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
