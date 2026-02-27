using UnityEngine;
using CursedDepths.Items;
using CursedDepths.Inventory;

namespace CursedDepths.Items
{
    public class Pickup : MonoBehaviour
    {
        public enum PickupType
        {
            Item,
            Gold,
            Health
        }

        public PickupType pickupType;
        public Item item;
        public int amount = 1;
        public float pickupRange = 1.5f;
        public float magnetSpeed = 5f;
        public bool magnetEnabled = true;

        private Transform player;
        private bool isMagnetizing = false;
        private SpriteRenderer spriteRenderer;
        private Collider2D collider;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
            
            if (spriteRenderer != null)
            {
                if (pickupType == PickupType.Gold)
                {
                    spriteRenderer.color = Color.yellow;
                }
                else if (pickupType == PickupType.Health)
                {
                    spriteRenderer.color = Color.red;
                }
            }
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= pickupRange || isMagnetizing)
            {
                if (magnetEnabled && distance < 5f)
                {
                    isMagnetizing = true;
                    Vector2 direction = (player.position - transform.position).normalized;
                    transform.Translate(direction * magnetSpeed * Time.deltaTime);
                }

                if (distance <= 0.5f)
                {
                    Collect();
                }
            }
        }

        private void Collect()
        {
            Player.PlayerController playerController = player.GetComponent<Player.PlayerController>();
            
            switch (pickupType)
            {
                case PickupType.Item:
                    if (item != null)
                    {
                        InventorySystem.Instance?.AddItem(item, amount);
                    }
                    break;
                case PickupType.Gold:
                    GameManager.Instance?.AddGold(amount);
                    break;
                case PickupType.Health:
                    playerController?.Heal(amount);
                    break;
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }
    }
}
