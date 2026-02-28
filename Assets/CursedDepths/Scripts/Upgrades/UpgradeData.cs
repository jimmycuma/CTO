using UnityEngine;

namespace CursedDepths.Upgrades
{
    [CreateAssetMenu(menuName = "CursedDepths/Upgrade", fileName = "Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea] public string description;
        public int bonusHealth;
        public int bonusDamage;
        public int bonusAmmo;
        public float bonusFireRate;
        public float bonusProjectileSpeed;
        public string synergyTag;
    }
}
