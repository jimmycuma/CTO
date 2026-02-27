using UnityEngine;
using UnityEngine.UI;

namespace CursedDepths.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("References")]
        public Slider healthSlider;
        public Slider healthFillSlider;
        public float fillSpeed = 5f;

        private float targetHealth;

        private void Start()
        {
            Core.EventManager.Instance.OnPlayerHealthUpdated += UpdateHealthBar;
            
            if (healthFillSlider != null)
            {
                healthFillSlider.maxValue = 100;
                healthFillSlider.value = 100;
                targetHealth = 100;
            }
        }

        private void OnDestroy()
        {
            Core.EventManager.Instance.OnPlayerHealthUpdated -= UpdateHealthBar;
        }

        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            targetHealth = (float)currentHealth / maxHealth * 100;
        }

        private void Update()
        {
            if (healthFillSlider != null)
            {
                healthFillSlider.value = Mathf.Lerp(healthFillSlider.value, targetHealth, fillSpeed * Time.deltaTime);
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
            }
        }

        public void SetHealth(int health)
        {
            if (healthSlider != null)
            {
                healthSlider.value = health;
            }
        }
    }
}
