using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Core;
using CursedDepths.Player;

namespace CursedDepths.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void Initialize(PlayerHealth health)
        {
            if (fillImage == null)
            {
                fillImage = GetComponent<Image>();
            }

            EventBus.PlayerHealthChanged += OnHealthChanged;
            OnHealthChanged(health.CurrentHealth, health.MaxHealth);
        }

        private void OnDestroy()
        {
            EventBus.PlayerHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = max > 0 ? (float)current / max : 0f;
            }
        }
    }
}
