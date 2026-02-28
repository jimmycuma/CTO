using System;

namespace CursedDepths.Core
{
    public static class EventBus
    {
        public static event Action<int, int> PlayerHealthChanged;
        public static event Action<int, int> PlayerAmmoChanged;
        public static event Action<int> WaveChanged;
        public static event Action<int> CurrencyChanged;
        public static event Action UpgradeSelectionStarted;
        public static event Action UpgradeSelectionEnded;
        public static event Action GameOver;

        public static void RaisePlayerHealthChanged(int current, int max)
        {
            PlayerHealthChanged?.Invoke(current, max);
        }

        public static void RaisePlayerAmmoChanged(int current, int max)
        {
            PlayerAmmoChanged?.Invoke(current, max);
        }

        public static void RaiseWaveChanged(int wave)
        {
            WaveChanged?.Invoke(wave);
        }

        public static void RaiseCurrencyChanged(int amount)
        {
            CurrencyChanged?.Invoke(amount);
        }

        public static void RaiseUpgradeSelectionStarted()
        {
            UpgradeSelectionStarted?.Invoke();
        }

        public static void RaiseUpgradeSelectionEnded()
        {
            UpgradeSelectionEnded?.Invoke();
        }

        public static void RaiseGameOver()
        {
            GameOver?.Invoke();
        }
    }
}
