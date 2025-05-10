using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RiwasGame.Utils
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { get; private set; }

        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;

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

        public IEnumerator FadeOut()
        {
            yield return Fade(1);
        }

        public IEnumerator FadeIn()
        {
            yield return Fade(0);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            fadeImage.gameObject.SetActive(true);

            Color color = fadeImage.color;
            float startAlpha = color.a;
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            fadeImage.color = new Color(color.r, color.g, color.b, targetAlpha);
            fadeImage.gameObject.SetActive(targetAlpha > 0);
        }

        // Fade → mark memory → load scene → fade in
        public void TransitionToScene(string sceneName, string memoryId = null, float delay = 0f)
        {
            StartCoroutine(DoSceneTransition(sceneName, memoryId, delay));
        }

        private IEnumerator DoSceneTransition(string sceneName, string memoryId, float delay)
        {
            // Disable player controls here if needed

            if (AudioManager.Instance != null)
                AudioManager.Instance.FadeOutMusic(0.5f);

            yield return FadeOut();

            if (!string.IsNullOrEmpty(memoryId) && MemoryManager.Instance != null)
                MemoryManager.Instance.MarkMemoryVisited(memoryId);

            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(sceneName);
            yield return null;

            yield return FadeIn();

            // Re-enable controls here if needed
        }
    }
}
