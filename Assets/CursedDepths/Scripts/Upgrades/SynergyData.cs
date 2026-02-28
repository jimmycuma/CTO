using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Upgrades
{
    [CreateAssetMenu(menuName = "CursedDepths/Synergy", fileName = "Synergy")]
    public class SynergyData : ScriptableObject
    {
        public string id;
        public List<string> requiredUpgradeIds = new List<string>();
        public string description;
        public int bonusDamage;
        public float bonusFireRate;
    }
}
