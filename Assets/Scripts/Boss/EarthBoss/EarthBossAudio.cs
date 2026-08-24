using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EarthBossAudio : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Boss Sounds")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip slamSound;
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private AudioClip deathSound;

        [Header("Volume")]
        [SerializeField] private float footstepVolume = 0.6f;
        [SerializeField] private float jumpVolume = 0.8f;
        [SerializeField] private float slamVolume = 1f;
        [SerializeField] private float hurtVolume = 0.8f;
        [SerializeField] private float deathVolume = 1f;

        public void PlayFootstep()
        {
            PlayRandom(
                footstepSounds,
                footstepVolume
            );
        }

        public void PlayJump()
        {
            PlaySound(
                jumpSound,
                jumpVolume
            );
        }

        public void PlaySlam()
        {
            PlaySound(
                slamSound,
                slamVolume
            );
        }

        public void PlayHurt()
        {
            PlaySound(
                hurtSound,
                hurtVolume
            );
        }

        public void PlayDeath()
        {
            PlaySound(
                deathSound,
                deathVolume
            );
        }

        private void PlayRandom(
            AudioClip[] clips,
            float volume)
        {
            if (audioSource == null)
                return;

            if (clips == null ||
                clips.Length == 0)
                return;

            int index =
                Random.Range(0, clips.Length);

            audioSource.PlayOneShot(
                clips[index],
                volume
            );
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