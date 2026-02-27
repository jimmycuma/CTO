using UnityEngine;
using CursedDepths.Inventory;

namespace CursedDepths.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "CursedDepths/Item")]
    public class Item : ScriptableObject
    {
        public enum ItemType
        {
            Consumable,
            Weapon,
            Armor,
            Key,
            Material
        }

        public string itemName;
        public Sprite icon;
        [TextArea] public string description;
        public ItemType itemType;
        public int stackSize = 1;
        public int goldValue = 10;

        public virtual void Use(InventorySystem inventory)
        {
            Debug.Log($"Used {itemName}");
        }
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "CursedDepths/Weapon")]
    public class Weapon : Item
    {
        public int damageBonus = 5;
        public float attackSpeedBonus = 1.0f;

        private void OnEnable()
        {
            itemType = ItemType.Weapon;
        }

        public override void Use(InventorySystem inventory)
        {
            Debug.Log($"Equipped {itemName} - Damage: +{damageBonus}");
        }
    }

    [CreateAssetMenu(fileName = "New Armor", menuName = "CursedDepths/Armor")]
    public class Armor : Item
    {
        public int defenseBonus = 5;

        private void OnEnable()
        {
            itemType = ItemType.Armor;
        }

        public override void Use(InventorySystem inventory)
        {
            Debug.Log($"Equipped {itemName} - Defense: +{defenseBonus}");
        }
    }

    [CreateAssetMenu(fileName = "New Potion", menuName = "CursedDepths/Potion")]
    public class Potion : Item
    {
        public enum PotionType
        {
            Health,
            Mana,
            Speed,
            Strength
        }

        public PotionType potionType;
        public int effectAmount = 25;

        private void OnEnable()
        {
            itemType = ItemType.Consumable;
        }

        public override void Use(InventorySystem inventory)
        {
            Player.PlayerController player = Player.PlayerController.FindObjectOfType<Player.PlayerController>();
            
            if (player != null)
            {
                switch (potionType)
                {
                    case PotionType.Health:
                        player.Heal(effectAmount);
                        break;
                    case PotionType.Mana:
                        Debug.Log($"Restored {effectAmount} mana");
                        break;
                    case PotionType.Speed:
                        Debug.Log($"Increased speed");
                        break;
                    case PotionType.Strength:
                        Debug.Log($"Increased strength");
                        break;
                }
                
                Debug.Log($"Used {itemName}");
            }
        }
    }

    [CreateAssetMenu(fileName = "New Key", menuName = "CursedDepths/Key")]
    public class KeyItem : Item
    {
        public string keyId;

        private void OnEnable()
        {
            itemType = ItemType.Key;
        }

        public override void Use(InventorySystem inventory)
        {
            Debug.Log($"Used key: {keyId}");
        }
    }
}
