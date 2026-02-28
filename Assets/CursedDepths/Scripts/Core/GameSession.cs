using System.Collections.Generic;
using UnityEngine;
using CursedDepths.Player;
using CursedDepths.Waves;
using CursedDepths.Upgrades;
using CursedDepths.Meta;
using CursedDepths.Audio;

namespace CursedDepths.Core
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private WaveManager waveManagerPrefab;
        [SerializeField] private PoolManager poolManagerPrefab;
        [SerializeField] private UI.UIManager uiManagerPrefab;
        [SerializeField] private AudioManager audioManagerPrefab;

        [Header("Data")]
        [SerializeField] private PlayerStatsData playerStats;
        [SerializeField] private List<WaveConfig> waveConfigs = new List<WaveConfig>();
        [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();
        [SerializeField] private List<SynergyData> synergies = new List<SynergyData>();

        private PlayerController playerInstance;
        private WaveManager waveManagerInstance;
        private PoolManager poolManagerInstance;
        private UI.UIManager uiManagerInstance;
        private AudioManager audioManagerInstance;
        private UpgradeManager upgradeManager;
        private MetaProgress metaProgress;

        public PlayerController Player => playerInstance;
        public PoolManager PoolManager => poolManagerInstance;
        public UpgradeManager UpgradeManager => upgradeManager;
        public MetaProgress MetaProgress => metaProgress;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            metaProgress = new MetaProgress();
            metaProgress.Load();

            poolManagerInstance = Instantiate(poolManagerPrefab);
            playerInstance = Instantiate(playerPrefab);
            playerInstance.Initialize(playerStats);

            waveManagerInstance = Instantiate(waveManagerPrefab);
            waveManagerInstance.Initialize(waveConfigs, playerInstance.transform);

            upgradeManager = gameObject.AddComponent<UpgradeManager>();
            upgradeManager.Initialize(upgrades, synergies, playerInstance, metaProgress);

            uiManagerInstance = Instantiate(uiManagerPrefab);
            uiManagerInstance.Initialize(playerInstance, waveManagerInstance, upgradeManager, metaProgress);

            audioManagerInstance = Instantiate(audioManagerPrefab);
            audioManagerInstance.Initialize();
        }

        public void RegisterEnemyKill(int currencyAmount)
        {
            metaProgress.AddCurrency(currencyAmount);
        }

        public void GameOver()
        {
            EventBus.RaiseGameOver();
            metaProgress.Save();
        }
    }
}
