using UnityEngine;
using UnityEngine.SceneManagement;

namespace CursedDepths.Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private string mainSceneName = "MainGame";

        private void Awake()
        {
            if (FindObjectsOfType<GameBootstrapper>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
