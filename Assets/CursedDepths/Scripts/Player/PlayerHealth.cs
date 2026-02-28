using UnityEngine;
using CursedDepths.Core;
using CursedDepths.Combat;

namespace CursedDepths.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        private int maxHealth;
        private int currentHealth;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;

        public void ApplyStats(PlayerStatsData data)
        {
            maxHealth = data.maxHealth;
            currentHealth = maxHealth;
            EventBus.RaisePlayerHealthChanged(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            EventBus.RaisePlayerHealthChanged(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                GameSession.Instance.GameOver();
            }
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            EventBus.RaisePlayerHealthChanged(currentHealth, maxHealth);
        }

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
            currentHealth = maxHealth;
            EventBus.RaisePlayerHealthChanged(currentHealth, maxHealth);
        }
    }
}
