using UnityEngine;
using CursedDepths.Core;

namespace CursedDepths.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        public void Initialize()
        {
            if (root == null)
            {
                root = gameObject;
            }

            EventBus.GameOver += Show;
            root.SetActive(false);
        }

        private void OnDestroy()
        {
            EventBus.GameOver -= Show;
        }

        private void Show()
        {
            root.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
