using System.Collections.Generic;
using UnityEngine;
using CursedDepths.Enemies;
using CursedDepths.Core;

namespace CursedDepths.Waves
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private MeleeEnemy meleePrefab;
        [SerializeField] private RangedEnemy rangedPrefab;
        [SerializeField] private BossEnemy bossPrefab;
        [SerializeField] private float spawnRadius = 6f;

        private readonly List<WaveConfig> waves = new List<WaveConfig>();
        private Transform target;
        private int currentWaveIndex = -1;
        private float spawnTimer;
        private int spawnEntryIndex;
        private int spawnedCount;
        private int totalToSpawn;
        private WaveConfig activeWave;

        public int CurrentWave => currentWaveIndex + 1;

        public void Initialize(List<WaveConfig> configs, Transform targetTransform)
        {
            waves.Clear();
            waves.AddRange(configs);
            target = targetTransform;
            AdvanceWave();
        }

        private void Update()
        {
            if (activeWave == null || target == null)
            {
                return;
            }

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && spawnedCount < totalToSpawn)
            {
                SpawnNext();
                spawnTimer = activeWave.spawnInterval;
            }

            if (spawnedCount >= totalToSpawn && EnemyBase.ActiveCount == 0)
            {
                AdvanceWave();
            }
        }

        private void AdvanceWave()
        {
            currentWaveIndex++;
            if (currentWaveIndex >= waves.Count)
            {
                currentWaveIndex = waves.Count - 1;
            }

            activeWave = waves[currentWaveIndex];
            spawnEntryIndex = 0;
            spawnedCount = 0;
            totalToSpawn = 0;

            for (int i = 0; i < activeWave.spawns.Count; i++)
            {
                totalToSpawn += activeWave.spawns[i].count;
            }

            EventBus.RaiseWaveChanged(activeWave.waveIndex);
        }

        private void SpawnNext()
        {
            if (spawnEntryIndex >= activeWave.spawns.Count)
            {
                return;
            }

            var entry = activeWave.spawns[spawnEntryIndex];
            if (spawnedCount >= totalToSpawn)
            {
                return;
            }

            var enemy = CreateEnemy(entry.enemyType);
            if (enemy != null)
            {
                enemy.Initialize(target);
            }

            spawnedCount++;
            entry.count--;
            if (entry.count <= 0)
            {
                spawnEntryIndex++;
            }
        }

        private EnemyBase CreateEnemy(EnemyType type)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = target.position + new Vector3(offset.x, offset.y, 0f);

            switch (type)
            {
                case EnemyType.Ranged:
                    return Instantiate(rangedPrefab, spawnPosition, Quaternion.identity);
                case EnemyType.Boss:
                    return Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
                default:
                    return Instantiate(meleePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
