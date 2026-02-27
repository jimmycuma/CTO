using System.Collections.Generic;
using UnityEngine;
using CursedDepths.Items;

namespace CursedDepths.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }

        [Header("Inventory Settings")]
        public int maxSlots = 20;

        [System.Serializable]
        public class InventorySlot
        {
            public Item item;
            public int quantity;

            public InventorySlot(Item item, int quantity)
            {
                this.item = item;
                this.quantity = quantity;
            }
        }

        public List<InventorySlot> inventory = new List<InventorySlot>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool AddItem(Item item, int quantity = 1)
        {
            if (quantity <= 0) return false;

            InventorySlot existingSlot = inventory.Find(slot => slot.item == item);

            if (existingSlot != null)
            {
                existingSlot.quantity += quantity;
            }
            else
            {
                if (inventory.Count >= maxSlots)
                {
                    return false;
                }
                inventory.Add(new InventorySlot(item, quantity));
            }

            Core.EventManager.Instance?.ItemCollected(item.itemName);
            return true;
        }

        public bool RemoveItem(Item item, int quantity = 1)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            
            if (slot == null) return false;

            slot.quantity -= quantity;

            if (slot.quantity <= 0)
            {
                inventory.Remove(slot);
            }

            return true;
        }

        public int GetItemCount(Item item)
        {
            InventorySlot slot = inventory.Find(s => s.item == item);
            return slot != null ? slot.quantity : 0;
        }

        public bool HasItem(Item item)
        {
            return inventory.Exists(s => s.item == item);
        }

        public void UseItem(Item item)
        {
            if (item == null) return;

            item.Use(this);
            RemoveItem(item, 1);
        }

        public int GetAttackBonus()
        {
            int bonus = 0;
            
            foreach (var slot in inventory)
            {
                if (slot.item != null && slot.item.itemType == Item.ItemType.Weapon)
                {
                    Weapon weapon = slot.item as Weapon;
                    if (weapon != null)
                    {
                        bonus += weapon.damageBonus;
                    }
                }
            }
            
            return bonus;
        }

        public int GetDefenseBonus()
        {
            int bonus = 0;
            
            foreach (var slot in inventory)
            {
                if (slot.item != null && slot.item.itemType == Item.ItemType.Armor)
                {
                    Armor armor = slot.item as Armor;
                    if (armor != null)
                    {
                        bonus += armor.defenseBonus;
                    }
                }
            }
            
            return bonus;
        }
    }
}
