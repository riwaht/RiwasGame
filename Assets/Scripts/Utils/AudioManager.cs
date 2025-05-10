using UnityEngine;
using System.Collections;

namespace RiwasGame.Utils
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("General")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

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


        public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
        {
            if (musicSource.clip == clip) return;

            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void FadeOutMusic(float duration = 1f)
        {
            StartCoroutine(FadeMusic(0f, duration));
        }

        public void FadeInMusic(AudioClip clip, float targetVolume = 1f, float duration = 1f)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeMusic(targetVolume, duration));
        }

        private IEnumerator FadeMusic(float targetVolume, float duration)
        {
            float startVolume = musicSource.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
                yield return null;
            }

            musicSource.volume = targetVolume;

            if (targetVolume == 0f)
                musicSource.Stop();
        }


        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            sfxSource.PlayOneShot(clip, volume);
        }

        public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
}
