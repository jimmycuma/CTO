using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Waves
{
    [CreateAssetMenu(menuName = "CursedDepths/Wave Config", fileName = "WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public int waveIndex = 1;
        public bool isBossWave;
        public float spawnInterval = 1.2f;
        public List<WaveSpawnEntry> spawns = new List<WaveSpawnEntry>();
    }

    [System.Serializable]
    public class WaveSpawnEntry
    {
        public EnemyType enemyType;
        public int count = 5;
    }

    public enum EnemyType
    {
        Melee,
        Ranged,
        Boss
    }
}
