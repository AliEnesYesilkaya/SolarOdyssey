using UnityEngine;

namespace SolarOdyssey.Audio
{
    public class EnvironmentAudio : MonoBehaviour
    {
        public static EnvironmentAudio Instance { get; private set; }

        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Sounds")]
        [SerializeField] private AudioClip goldPickupSound;
        [SerializeField] private AudioClip heartPickupSound;
        [SerializeField] private AudioClip waterSplashSound;
        [SerializeField] private AudioClip checkpointSound;

        [Header("Volume")]
        [SerializeField] private float goldVolume = 0.8f;
        [SerializeField] private float heartVolume = 0.8f;
        [SerializeField] private float waterVolume = 0.8f;
        [SerializeField] private float checkpointVolume = 0.8f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void PlayGold()
        {
            PlaySound(goldPickupSound, goldVolume);
        }

        public void PlayHeart()
        {
            PlaySound(heartPickupSound, heartVolume);
        }

        public void PlayWaterSplash()
        {
            PlaySound(waterSplashSound, waterVolume);
        }

        public void PlayCheckpoint()
        {
            PlaySound(checkpointSound, checkpointVolume);
        }

        private void PlaySound(
            AudioClip clip,
            float volume)
        {
            if (audioSource == null)
                return;

            if (clip == null)
                return;

            audioSource.PlayOneShot(
                clip,
                volume
            );
        }
    }
}