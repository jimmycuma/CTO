using UnityEngine;

namespace CursedDepths.Player
{
    [CreateAssetMenu(menuName = "CursedDepths/Player Stats", fileName = "PlayerStats")]
    public class PlayerStatsData : ScriptableObject
    {
        public int maxHealth = 100;
        public float moveSpeed = 5f;
        public int maxAmmo = 8;
        public float fireRate = 0.35f;
        public int damage = 10;
        public float projectileSpeed = 12f;
        public float reloadTime = 1.2f;
    }
}
