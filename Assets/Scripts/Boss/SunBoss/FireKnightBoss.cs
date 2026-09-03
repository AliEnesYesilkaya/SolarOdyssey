using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class FireKnightBoss : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;

        [Header("Detection")]
        [SerializeField] private float detectionRange = 10f;

        [Header("Ranged Attack")]
        [SerializeField] private float rangedMinDistance = 5f;
        [SerializeField] private float rangedMaxDistance = 10f;
        [SerializeField] private int rangedAttackDamage = 15;
        [SerializeField] private float rangedAttackCooldown = 1.5f;

        [Header("Melee Attack")]
        [SerializeField] private float meleeAttackRange = 2f;
        [SerializeField] private float meleeAttackCooldown = 1f;

        [Header("Melee Attack Damages")]
        [SerializeField] private int attack1Damage = 10;
        [SerializeField] private int attack2Damage = 20;
        [SerializeField] private int attack3Damage = 15;

        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private Transform player;

        private float attackTimer;

        private bool isAttacking;
        private bool isDead;

        private void Awake()
        {
            rb =
                GetComponent<Rigidbody2D>();

            animator =
                GetComponentInChildren<Animator>();

            spriteRenderer =
                GetComponentInChildren<SpriteRenderer>();

            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player =
                    playerObject.transform;
            }

            attackTimer = 0f;
        }

        private void FixedUpdate()
        {
            if (isDead)
                return;

            if (player == null)
                return;

            if (isAttacking)
            {
                rb.linearVelocity =
                    new Vector2(
                        0f,
                        rb.linearVelocity.y
                    );

                UpdateAnimator();
                return;
            }

            attackTimer -= Time.fixedDeltaTime;

            float distance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            // =========================================
            // OYUNCU ÇOK UZAK
            // =========================================

            if (distance > detectionRange)
            {
                StopMoving();
                return;
            }

            // =========================================
            // UZAK SALDIRI
            // 5 < MESAFE <= 10
            // =========================================

            if (distance > rangedMinDistance &&
                distance <= rangedMaxDistance)
            {
                StopMoving();

                FacePlayer();

                if (attackTimer <= 0f)
                {
                    StartRangedAttack();
                }

                return;
            }

            // =========================================
            // YAKIN MESAFE
            // MESAFE <= 5
            // =========================================

            FacePlayer();

            if (distance > meleeAttackRange)
            {
                MoveTowardsPlayer();
                return;
            }

            // =========================================
            // YAKIN SALDIRI
            // =========================================

            StopMoving();

            if (attackTimer <= 0f)
            {
                StartRandomMeleeAttack();
            }

            UpdateAnimator();
        }

        // =============================================
        // OYUNCUYA DOĞRU HAREKET
        // =============================================

        private void MoveTowardsPlayer()
        {
            if (player == null)
                return;

            float direction =
                Mathf.Sign(
                    player.position.x -
                    transform.position.x
                );

            rb.linearVelocity =
                new Vector2(
                    direction * moveSpeed,
                    rb.linearVelocity.y
                );

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    1f
                );
            }

            FacePlayer();
        }

        // =============================================
        // DUR
        // =============================================

        private void StopMoving()
        {
            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );
            }
        }

        // =============================================
        // OYUNCUYA BAK
        // =============================================

        private void FacePlayer()
        {
            if (player == null)
                return;

            if (spriteRenderer == null)
                return;

            if (player.position.x >
                transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (player.position.x <
                     transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
        }

        // =============================================
        // UZAK SALDIRI
        // =============================================

        private void StartRangedAttack()
        {
            isAttacking = true;

            attackTimer =
                rangedAttackCooldown;

            StopMoving();

            if (animator != null)
            {
                animator.SetTrigger(
                    "RangedAttack"
                );
            }

            Debug.Log(
                "Fire Knight uzak saldırı başladı."
            );
        }

        // =============================================
        // RASTGELE YAKIN SALDIRI
        // =============================================

        private void StartRandomMeleeAttack()
        {
            isAttacking = true;

            attackTimer =
                meleeAttackCooldown;

            StopMoving();

            float randomValue =
                Random.value;

            // %30 ZAYIF
            if (randomValue < 0.30f)
            {
                if (animator != null)
                {
                    animator.SetTrigger(
                        "Attack1"
                    );
                }

                Debug.Log(
                    "Fire Knight Attack 1 seçti. Hasar: " +
                    attack1Damage
                );

                return;
            }

            // %40 ORTA
            if (randomValue < 0.70f)
            {
                if (animator != null)
                {
                    animator.SetTrigger(
                        "Attack3"
                    );
                }

                Debug.Log(
                    "Fire Knight Attack 3 seçti. Hasar: " +
                    attack3Damage
                );

                return;
            }

            // %30 GÜÇLÜ
            if (animator != null)
            {
                animator.SetTrigger(
                    "Attack2"
                );
            }

            Debug.Log(
                "Fire Knight Attack 2 seçti. Hasar: " +
                attack2Damage
            );
        }

        // =============================================
        // ATTACK 1 HASARI
        // =============================================

        public void Attack1Hit()
        {
            DealMeleeDamage(
                attack1Damage
            );
        }

        // =============================================
        // ATTACK 2 HASARI
        // =============================================

        public void Attack2Hit()
        {
            DealMeleeDamage(
                attack2Damage
            );
        }

        // =============================================
        // ATTACK 3 HASARI
        // =============================================

        public void Attack3Hit()
        {
            DealMeleeDamage(
                attack3Damage
            );
        }

        // =============================================
        // YAKIN SALDIRI HASARI
        // =============================================

        private void DealMeleeDamage(
            int damage)
        {
            if (player == null)
                return;

            float distance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            if (distance > meleeAttackRange)
                return;

            Health health =
                player.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(
                    damage
                );

                Debug.Log(
                    "Fire Knight yakın saldırı: " +
                    damage +
                    " hasar."
                );
            }
        }

        // =============================================
        // UZAK SALDIRI HASARI
        // =============================================

        public void RangedAttackHit()
        {
            if (player == null)
                return;

            float distance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            if (distance <= rangedMinDistance ||
                distance > rangedMaxDistance)
                return;

            Health health =
                player.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(
                    rangedAttackDamage
                );

                Debug.Log(
                    "Fire Knight uzak saldırı: " +
                    rangedAttackDamage +
                    " hasar."
                );
            }
        }

        // =============================================
        // SALDIRI BİTTİ
        // =============================================

        public void AttackFinished()
        {
            isAttacking = false;

            Debug.Log(
                "Fire Knight saldırı bitti."
            );
        }

        // =============================================
        // UZAK SALDIRI BİTTİ
        // =============================================

        public void RangedAttackFinished()
        {
            isAttacking = false;

            Debug.Log(
                "Fire Knight uzak saldırı bitti."
            );
        }

        // =============================================
        // ANIMATOR
        // =============================================

        private void UpdateAnimator()
        {
            if (animator == null)
                return;

            if (isAttacking)
            {
                animator.SetFloat(
                    "Speed",
                    1f
                );

                return;
            }

            animator.SetFloat(
                "Speed",
                Mathf.Abs(
                    rb.linearVelocity.x
                )
            );
        }

        // =============================================
        // DEATH
        // =============================================

        public void SetDead()
        {
            if (isDead)
                return;

            isDead = true;
            isAttacking = false;

            rb.linearVelocity =
                Vector2.zero;

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );

                animator.SetTrigger(
                    "Dead"
                );
            }

            Debug.Log(
                "Fire Knight öldü."
            );
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}