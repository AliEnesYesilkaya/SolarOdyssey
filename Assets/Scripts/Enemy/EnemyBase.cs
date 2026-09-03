using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float detectionRange = 5f;
        [SerializeField] protected float attackRange = 1.2f;
        [SerializeField] protected int attackDamage = 20;

        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip[] footstepSounds;
        [SerializeField] protected AudioClip deathSound;

        [SerializeField] protected float footstepVolume = 0.1f;
        [SerializeField] protected float deathVolume = 0.8f;
        [SerializeField] protected float footstepInterval = 0.3f;

        private float footstepTimer;

        protected Transform player;
        protected Rigidbody2D rb;
        protected Animator animator;

        protected StateMachine stateMachine;

        private Vector3 startPosition;
        private Quaternion startRotation;

        protected virtual void Awake()
        {
            // Oyunun ilk açıldığı konumu kaydet.
            startPosition = transform.position;
            startRotation = transform.rotation;

            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            rb = GetComponent<Rigidbody2D>();

            animator =
                GetComponentInChildren<Animator>();

            stateMachine =
                new StateMachine();

            // Ortak sesleri üst Enemy objesinden bul.
            EnemyBase[] enemyBases =
                GetComponentsInParent<EnemyBase>(true);

            foreach (EnemyBase enemy in enemyBases)
            {
                if (enemy == this)
                    continue;

                if (audioSource == null)
                    audioSource = enemy.audioSource;

                if (footstepSounds == null ||
                    footstepSounds.Length == 0)
                {
                    footstepSounds =
                        enemy.footstepSounds;
                }

                if (deathSound == null)
                    deathSound = enemy.deathSound;

                footstepVolume =
                    enemy.footstepVolume;

                deathVolume =
                    enemy.deathVolume;

                footstepInterval =
                    enemy.footstepInterval;

                break;
            }
        }

        protected virtual void Update()
        {
            // Enemy2 patrol halindeyken yön kontrolü tamamen
            // Enemy2PatrolState tarafından yapılıyor; FacePlayer()
            // bunu ezmesin diye bu durumda çağrılmıyor.
            if (stateMachine?.CurrentState is not Enemy2PatrolState)
            {
                FacePlayer();
            }

            HandleFootsteps();
        }

        protected virtual void FixedUpdate()
        {
            stateMachine?.FixedUpdate();
        }

        private void HandleFootsteps()
        {
            if (audioSource == null)
                return;

            if (footstepSounds == null ||
                footstepSounds.Length == 0)
                return;

            if (rb == null)
                return;

            if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
            {
                footstepTimer = 0f;
                return;
            }

            if (Mathf.Abs(rb.linearVelocity.y) > 0.1f)
            {
                footstepTimer = 0f;
                return;
            }

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();

                footstepTimer =
                    footstepInterval;
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
                Random.Range(
                    0,
                    footstepSounds.Length
                );

            audioSource.PlayOneShot(
                footstepSounds[index],
                footstepVolume
            );
        }

        public void PlayDeath()
        {
            if (audioSource == null)
                return;

            if (deathSound == null)
                return;

            audioSource.PlayOneShot(
                deathSound,
                deathVolume
            );
        }

        protected void FacePlayer()
        {
            if (player == null)
                return;

            Transform visual =
                transform.Find("Visual");

            if (visual == null)
                return;

            float direction =
                Mathf.Sign(
                    player.position.x -
                    transform.position.x
                );

            Vector3 scale =
                visual.localScale;

            scale.x =
                Mathf.Abs(scale.x) *
                direction;

            visual.localScale = scale;
        }

        public void AttackHit()
        {
            if (player == null)
                return;

            float distance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            if (distance > attackRange)
                return;

            Health health =
                player.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(
                    attackDamage
                );

                Debug.Log(
                    gameObject.name +
                    " oyuncuya " +
                    attackDamage +
                    " hasar verdi."
                );
            }
        }

        //reset world
        public virtual void ResetToStart()
        {
            // Önce GameObject'i tekrar aç.
            gameObject.SetActive(true);

            // Enemy scripti ölümde kapatılmış olabilir.
            enabled = true;

            // Fizik hareketini sıfırla.
            if (rb != null)
            {
                rb.linearVelocity =
                    Vector2.zero;

                rb.angularVelocity = 0f;
            }

            // İlk konuma dön.
            transform.position =
                startPosition;

            transform.rotation =
                startRotation;

            footstepTimer = 0f;

            // Health'i tamamen sıfırla.
            Health health =
                GetComponent<Health>();

            if (health != null)
            {
                health.ResetHealth();
            }
        }
    }
}