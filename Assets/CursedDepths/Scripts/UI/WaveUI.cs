using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Core;
using CursedDepths.Waves;

namespace CursedDepths.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private Text waveText;

        public void Initialize(WaveManager waveManager)
        {
            if (waveText == null)
            {
                waveText = GetComponent<Text>();
            }

            EventBus.WaveChanged += OnWaveChanged;
            OnWaveChanged(waveManager.CurrentWave);
        }

        private void OnDestroy()
        {
            EventBus.WaveChanged -= OnWaveChanged;
        }

        private void OnWaveChanged(int wave)
        {
            if (waveText != null)
            {
                waveText.text = $"Wave {wave}";
            }
        }
    }
}
