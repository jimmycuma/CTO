using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Core;
using CursedDepths.Meta;

namespace CursedDepths.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private Text currencyText;

        public void Initialize(MetaProgress progress)
        {
            if (currencyText == null)
            {
                currencyText = GetComponent<Text>();
            }

            EventBus.CurrencyChanged += OnCurrencyChanged;
            OnCurrencyChanged(progress.Currency);
        }

        private void OnDestroy()
        {
            EventBus.CurrencyChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(int amount)
        {
            if (currencyText != null)
            {
                currencyText.text = $"Gold: {amount}";
            }
        }
    }
}
