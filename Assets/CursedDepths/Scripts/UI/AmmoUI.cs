using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Core;
using CursedDepths.Player;

namespace CursedDepths.UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private Text ammoText;

        public void Initialize(PlayerShooting shooting)
        {
            if (ammoText == null)
            {
                ammoText = GetComponent<Text>();
            }

            EventBus.PlayerAmmoChanged += OnAmmoChanged;
            OnAmmoChanged(shooting.CurrentAmmo, shooting.MaxAmmo);
        }

        private void OnDestroy()
        {
            EventBus.PlayerAmmoChanged -= OnAmmoChanged;
        }

        private void OnAmmoChanged(int current, int max)
        {
            if (ammoText != null)
            {
                ammoText.text = $"Ammo: {current}/{max}";
            }
        }
    }
}
