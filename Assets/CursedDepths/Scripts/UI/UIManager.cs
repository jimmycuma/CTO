using UnityEngine;
using CursedDepths.Player;
using CursedDepths.Waves;
using CursedDepths.Upgrades;
using CursedDepths.Meta;
using CursedDepths.Core;

namespace CursedDepths.UI
{
    public class UIManager : MonoBehaviour
    {
        private PlayerController player;
        private WaveManager waveManager;
        private UpgradeManager upgradeManager;
        private MetaProgress metaProgress;

        private int currentHealth;
        private int maxHealth;
        private int currentAmmo;
        private int maxAmmo;
        private int currentWave;
        private int currency;

        private UpgradeData[] currentOptions;
        private bool showingUpgrade;

        public void Initialize(PlayerController playerController, WaveManager manager, UpgradeManager upgrades, MetaProgress progress)
        {
            player = playerController;
            waveManager = manager;
            upgradeManager = upgrades;
            metaProgress = progress;

            currentHealth = player.Health.CurrentHealth;
            maxHealth = player.Health.MaxHealth;
            currentAmmo = player.Shooting.CurrentAmmo;
            maxAmmo = player.Shooting.MaxAmmo;
            currentWave = waveManager.CurrentWave;
            currency = metaProgress.Currency;

            EventBus.PlayerHealthChanged += OnHealthChanged;
            EventBus.PlayerAmmoChanged += OnAmmoChanged;
            EventBus.WaveChanged += OnWaveChanged;
            EventBus.CurrencyChanged += OnCurrencyChanged;
            EventBus.GameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            EventBus.PlayerHealthChanged -= OnHealthChanged;
            EventBus.PlayerAmmoChanged -= OnAmmoChanged;
            EventBus.WaveChanged -= OnWaveChanged;
            EventBus.CurrencyChanged -= OnCurrencyChanged;
            EventBus.GameOver -= OnGameOver;
        }

        private void OnHealthChanged(int current, int max)
        {
            currentHealth = current;
            maxHealth = max;
        }

        private void OnAmmoChanged(int current, int max)
        {
            currentAmmo = current;
            maxAmmo = max;
        }

        private void OnWaveChanged(int wave)
        {
            currentWave = wave;
            if (wave > 1)
            {
                ShowUpgradeSelection();
            }
        }

        private void OnCurrencyChanged(int amount)
        {
            currency = amount;
        }

        private void OnGameOver()
        {
            showingUpgrade = false;
        }

        private void ShowUpgradeSelection()
        {
            if (upgradeManager == null)
            {
                return;
            }

            currentOptions = upgradeManager.GetRandomSelection(3);
            showingUpgrade = true;
            Time.timeScale = 0f;
        }

        private void Update()
        {
            if (!showingUpgrade || currentOptions == null)
            {
                return;
            }

            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.digit1Key.wasPressedThisFrame)
            {
                SelectUpgrade(0);
            }
            else if (keyboard.digit2Key.wasPressedThisFrame)
            {
                SelectUpgrade(1);
            }
            else if (keyboard.digit3Key.wasPressedThisFrame)
            {
                SelectUpgrade(2);
            }
        }

        private void SelectUpgrade(int index)
        {
            if (currentOptions == null || index >= currentOptions.Length)
            {
                return;
            }

            upgradeManager.ApplyUpgrade(currentOptions[index]);
            showingUpgrade = false;
            Time.timeScale = 1f;
        }

        private void OnGUI()
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(16, 16, 300, 30), $"Wave {currentWave}");
            GUI.Label(new Rect(16, 46, 300, 30), $"Ammo: {currentAmmo}/{maxAmmo}");
            GUI.Label(new Rect(16, 76, 300, 30), $"Gold: {currency}");

            float healthPercent = maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
            GUI.Box(new Rect(16, 106, 200, 20), string.Empty);
            GUI.Box(new Rect(16, 106, 200 * healthPercent, 20), $"HP {currentHealth}/{maxHealth}");

            if (showingUpgrade && currentOptions != null)
            {
                GUI.Box(new Rect(Screen.width / 2f - 200, Screen.height / 2f - 140, 400, 280), "Choose an Upgrade (1-3)");
                for (int i = 0; i < currentOptions.Length; i++)
                {
                    var option = currentOptions[i];
                    GUI.Label(new Rect(Screen.width / 2f - 180, Screen.height / 2f - 100 + i * 70, 360, 60), $"{i + 1}. {option.displayName}\n{option.description}");
                }
            }
        }
    }
}
