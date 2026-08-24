using UnityEngine;

namespace SolarOdyssey.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Player Sounds")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip landSound;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private AudioClip deathSound;

        [Header("Volume")]
        [SerializeField] private float footstepVolume = 0.5f;
        [SerializeField] private float jumpVolume = 0.7f;
        [SerializeField] private float landVolume = 0.7f;
        [SerializeField] private float attackVolume = 0.8f;
        [SerializeField] private float hurtVolume = 0.8f;
        [SerializeField] private float deathVolume = 0.8f;

        [Header("Footstep Settings")]
        [SerializeField] private float footstepInterval = 0.3f;

        private float footstepTimer;

        private Rigidbody2D rb;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            HandleFootsteps();
        }

        private void HandleFootsteps()
        {
            if (audioSource == null)
                return;

            if (footstepSounds == null ||
                footstepSounds.Length == 0)
                return;

            if (playerMovement == null)
                return;

            // Havadaysak ayak sesi yok.
            if (!playerMovement.IsGrounded)
            {
                footstepTimer = 0f;
                return;
            }

            // Duvara yaslanıyorsak ayak sesi yok.
            if (playerMovement.IsTouchingWall)
            {
                footstepTimer = 0f;
                return;
            }

            // Gerçek yatay hareket yoksa ayak sesi yok.
            if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
            {
                footstepTimer = 0f;
                return;
            }

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }

        public void PlayFootstep()
        {
            if (audioSource == null)
                return;

            if (footstepSounds == null ||
                footstepSounds.Length == 0)
                return;

            int index =
                Random.Range(0, footstepSounds.Length);

            audioSource.PlayOneShot(
                footstepSounds[index],
                footstepVolume
            );
        }

        public void PlayJump()
        {
            PlaySound(jumpSound, jumpVolume);
        }

        public void PlayLand()
        {
            PlaySound(landSound, landVolume);
        }

        public void PlayAttack()
        {
            PlaySound(attackSound, attackVolume);
        }

        public void PlayHurt()
        {
            PlaySound(hurtSound, hurtVolume);
        }

        public void PlayDeath()
        {
            PlaySound(deathSound, deathVolume);
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