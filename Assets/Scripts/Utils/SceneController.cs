using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiwasGame.Utils
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(Transition(sceneName));
        }

        private System.Collections.IEnumerator Transition(string sceneName)
        {
            if (FadeManager.Instance != null)
                yield return FadeManager.Instance.FadeOut();

            SceneManager.LoadScene(sceneName);

            if (FadeManager.Instance != null)
                yield return FadeManager.Instance.FadeIn();
        }
    }
}
