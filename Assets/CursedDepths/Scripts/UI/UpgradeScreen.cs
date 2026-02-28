using UnityEngine;
using UnityEngine.UI;
using CursedDepths.Upgrades;
using CursedDepths.Core;

namespace CursedDepths.UI
{
    public class UpgradeScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button[] optionButtons;
        [SerializeField] private Text[] optionTexts;

        private UpgradeManager upgradeManager;
        private UpgradeData[] currentOptions;
        private bool awaitingSelection;

        public void Initialize(UpgradeManager manager)
        {
            upgradeManager = manager;
            if (root == null)
            {
                root = gameObject;
            }

            if (optionButtons == null || optionButtons.Length == 0)
            {
                optionButtons = GetComponentsInChildren<Button>(true);
            }

            if (optionTexts == null || optionTexts.Length == 0)
            {
                optionTexts = new Text[optionButtons.Length];
                for (int i = 0; i < optionButtons.Length; i++)
                {
                    optionTexts[i] = optionButtons[i].GetComponentInChildren<Text>(true);
                }
            }

            EventBus.UpgradeSelectionStarted += ShowSelection;
            EventBus.UpgradeSelectionEnded += Hide;
            Hide();
        }

        private void OnDestroy()
        {
            EventBus.UpgradeSelectionStarted -= ShowSelection;
            EventBus.UpgradeSelectionEnded -= Hide;
        }

        private void ShowSelection()
        {
            if (upgradeManager == null)
            {
                return;
            }

            int optionCount = optionButtons != null && optionButtons.Length > 0 ? optionButtons.Length : optionTexts.Length;
            currentOptions = upgradeManager.GetRandomSelection(optionCount);

            for (int i = 0; i < optionTexts.Length; i++)
            {
                if (i < currentOptions.Length && currentOptions[i] != null)
                {
                    optionTexts[i].text = $"{i + 1}. {currentOptions[i].displayName}\n{currentOptions[i].description}";
                    optionTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    optionTexts[i].gameObject.SetActive(false);
                }
            }

            if (optionButtons != null && optionButtons.Length > 0)
            {
                for (int i = 0; i < optionButtons.Length; i++)
                {
                    int index = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => SelectUpgrade(index));
                    optionButtons[i].gameObject.SetActive(i < currentOptions.Length);
                }
            }

            root.SetActive(true);
            awaitingSelection = true;
            Time.timeScale = 0f;
        }

        private void Update()
        {
            if (!awaitingSelection)
            {
                return;
            }

            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.digit1Key.wasPressedThisFrame)
                {
                    SelectUpgrade(0);
                }
                else if (keyboard.digit2Key.wasPressedThisFrame)
                {
                    SelectUpgrade(1);
                }
                else if (keyboard.digit3Key.wasPressedThisFrame)
                {
                    SelectUpgrade(2);
                }
            }
        }

        private void SelectUpgrade(int index)
        {
            if (currentOptions == null || index >= currentOptions.Length)
            {
                return;
            }

            upgradeManager.ApplyUpgrade(currentOptions[index]);
            Hide();
        }

        private void Hide()
        {
            awaitingSelection = false;
            root.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
