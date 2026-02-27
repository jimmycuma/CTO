using UnityEngine;
using System;

namespace CursedDepths.Core
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        public event Action<int> OnPlayerHealthChanged;
        public event Action<int, int> OnPlayerHealthUpdated;
        public event Action OnPlayerDeath;
        public event Action<int> OnEnemyKilled;
        public event Action<int> OnGoldCollected;
        public event Action<string> OnItemCollected;
        public event Action<int> OnLevelChanged;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameOver;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayerHealthChanged(int currentHealth)
        {
            OnPlayerHealthChanged?.Invoke(currentHealth);
        }

        public void PlayerHealthUpdated(int currentHealth, int maxHealth)
        {
            OnPlayerHealthUpdated?.Invoke(currentHealth, maxHealth);
        }

        public void PlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        public void EnemyKilled(int goldReward)
        {
            OnEnemyKilled?.Invoke(goldReward);
        }

        public void GoldCollected(int amount)
        {
            OnGoldCollected?.Invoke(amount);
        }

        public void ItemCollected(string itemName)
        {
            OnItemCollected?.Invoke(itemName);
        }

        public void LevelChanged(int level)
        {
            OnLevelChanged?.Invoke(level);
        }

        public void GamePaused()
        {
            OnGamePaused?.Invoke();
        }

        public void GameResumed()
        {
            OnGameResumed?.Invoke();
        }

        public void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}
