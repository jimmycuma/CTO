using UnityEngine;
using UnityEngine.SceneManagement;
using CursedDepths.Save;

namespace CursedDepths.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState currentState = GameState.MainMenu;
        public int currentLevel = 1;
        public int gold = 0;

        [Header("References")]
        public Player.PlayerController player;

        private bool isPaused = false;

        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver,
            Victory
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && currentState == GameState.Playing)
            {
                TogglePause();
            }
        }

        public void StartNewGame()
        {
            gold = 0;
            currentLevel = 1;
            currentState = GameState.Playing;
            SceneManager.LoadScene("Game");
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            currentState = GameState.MainMenu;
            SceneManager.LoadScene("MainMenu");
        }

        public void LoadGame()
        {
            SaveSystem.LoadGameData();
        }

        public void SaveGame()
        {
            SaveSystem.SaveGameData();
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
            currentState = GameState.Paused;
            EventManager.Instance?.GamePaused();
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            currentState = GameState.Playing;
            EventManager.Instance?.GameResumed();
        }

        public void GameOver()
        {
            currentState = GameState.GameOver;
            Time.timeScale = 1f;
            EventManager.Instance?.GameOver();
            SceneManager.LoadScene("GameOver");
        }

        public void AddGold(int amount)
        {
            gold += amount;
            EventManager.Instance?.GoldCollected(amount);
        }

        public void NextLevel()
        {
            currentLevel++;
            EventManager.Instance?.LevelChanged(currentLevel);
            SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetPlayer(Player.PlayerController playerRef)
        {
            player = playerRef;
        }
    }
}
