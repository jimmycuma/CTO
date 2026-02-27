using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Save;

namespace CursedDepths.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        public Button newGameButton;
        public Button continueButton;
        public Button quitButton;
        
        [Header("Panels")]
        public GameObject titlePanel;

        private void Start()
        {
            SetupButtons();
            CheckSaveFile();
        }

        private void SetupButtons()
        {
            if (newGameButton != null)
            {
                newGameButton.onClick.AddListener(StartNewGame);
            }

            if (continueButton != null)
            {
                continueButton.onClick.AddListener(ContinueGame);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(QuitGame);
            }
        }

        private void CheckSaveFile()
        {
            if (continueButton != null)
            {
                continueButton.interactable = SaveSystem.SaveExists();
            }
        }

        private void StartNewGame()
        {
            SaveSystem.DeleteSave();
            GameManager.Instance?.StartNewGame();
        }

        private void ContinueGame()
        {
            GameManager.Instance?.LoadGame();
            GameManager.Instance?.StartNewGame();
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
