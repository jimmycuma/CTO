using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CursedDepths.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("References")]
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI healthText;
        public GameObject pausePanel;
        public Button resumeButton;
        public Button saveButton;
        public Button mainMenuButton;
        public Button quitButton;

        private void Start()
        {
            Core.EventManager.Instance.OnGoldCollected += UpdateGoldDisplay;
            Core.EventManager.Instance.OnLevelChanged += UpdateLevelDisplay;
            Core.EventManager.Instance.OnGamePaused += ShowPauseMenu;
            Core.EventManager.Instance.OnGameResumed += HidePauseMenu;

            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            SetupButtons();
            UpdateDisplay();
        }

        private void OnDestroy()
        {
            Core.EventManager.Instance.OnGoldCollected -= UpdateGoldDisplay;
            Core.EventManager.Instance.OnLevelChanged -= UpdateLevelDisplay;
            Core.EventManager.Instance.OnGamePaused -= ShowPauseMenu;
            Core.EventManager.Instance.OnGameResumed -= HidePauseMenu;
        }

        private void SetupButtons()
        {
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(() => GameManager.Instance?.ResumeGame());
            }

            if (saveButton != null)
            {
                saveButton.onClick.AddListener(() => GameManager.Instance?.SaveGame());
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(() => GameManager.Instance?.LoadMainMenu());
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(QuitGame);
            }
        }

        private void UpdateGoldDisplay(int amount)
        {
            UpdateDisplay();
        }

        private void UpdateLevelDisplay(int level)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (GameManager.Instance != null)
            {
                if (goldText != null)
                {
                    goldText.text = "Gold: " + GameManager.Instance.gold;
                }

                if (levelText != null)
                {
                    levelText.text = "Level: " + GameManager.Instance.currentLevel;
                }
            }
        }

        private void ShowPauseMenu()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }

        private void HidePauseMenu()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }

        private void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
