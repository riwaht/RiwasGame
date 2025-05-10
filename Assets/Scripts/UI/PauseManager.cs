using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RiwasGame.UI
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance { get; private set; }

        [SerializeField] private GameObject pauseMenuUI;
        private bool isPaused = false;

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

        private void Update()
        {
            // Classic input method
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (isPaused) Resume();
                else Pause();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenuUI.SetActive(true);
            AudioListener.pause = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenuUI.SetActive(false);
            AudioListener.pause = false;
        }

        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}
