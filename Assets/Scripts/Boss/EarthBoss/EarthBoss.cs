using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EarthBoss : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;

        [Header("Patrol Points")]
        [SerializeField] private Transform patrolPointA;
        [SerializeField] private Transform patrolPointB;

        [Header("Player Detection")]
        [SerializeField] private float detectionRange = 20f;

        [Header("Boss Attack")]
        [SerializeField] private float attackCooldown = 3f;
        [SerializeField] private float attackRange = 15f;
        [SerializeField] private int attackDamage = 15;

        [Header("Boss Jump")]
        [SerializeField] private float jumpForce = 8f;

        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private Transform player;
        private PlayerMovement playerMovement;

        private EarthBossAudio bossAudio;

        private StateMachine stateMachine;

        private EarthBossPatrolState patrolState;
        private EarthBossAttackState attackState;

        private float attackTimer;

        private bool isDead;
        private bool isGrounded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            animator = GetComponentInChildren<Animator>();

            spriteRenderer =
                GetComponentInChildren<SpriteRenderer>();

            bossAudio =
                GetComponent<EarthBossAudio>();

            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;

                playerMovement =
                    playerObject.GetComponent<PlayerMovement>();
            }

            stateMachine = new StateMachine();

            patrolState =
                new EarthBossPatrolState(
                    rb,
                    transform,
                    patrolPointA,
                    patrolPointB,
                    moveSpeed
                );

            attackState =
                new EarthBossAttackState(
                    rb,
                    transform,
                    player,
                    animator,
                    playerMovement,
                    jumpForce,
                    attackRange,
                    attackDamage,
                    bossAudio
                );

            attackTimer = attackCooldown;
        }

        private void Start()
        {
            stateMachine.Initialize(patrolState);
        }

        private void FixedUpdate()
        {
            if (isDead)
                return;

            attackTimer -= Time.fixedDeltaTime;

            if (PlayerDetected())
            {
                if (attackTimer <= 0f &&
                    stateMachine.CurrentState != attackState)
                {
                    attackTimer = attackCooldown;

                    stateMachine.ChangeState(
                        attackState
                    );
                }
            }

            if (stateMachine.CurrentState == attackState &&
                !attackState.IsAttacking())
            {
                stateMachine.ChangeState(
                    patrolState
                );
            }

            stateMachine.FixedUpdate();

            UpdateAnimator();
            UpdateVisualDirection();
        }

        private bool PlayerDetected()
        {
            if (player == null)
                return false;

            float distance =
                Mathf.Abs(
                    player.position.x -
                    transform.position.x
                );

            return distance <= detectionRange;
        }

        private void UpdateAnimator()
        {
            if (animator == null)
                return;

            animator.SetFloat(
                "Speed",
                Mathf.Abs(rb.linearVelocity.x)
            );
        }

        private void UpdateVisualDirection()
        {
            if (spriteRenderer == null)
                return;

            if (rb.linearVelocity.x > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.linearVelocity.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }

        private void OnCollisionEnter2D(
            Collision2D collision)
        {
            if (collision.contacts.Length == 0)
                return;

            foreach (ContactPoint2D contact
                     in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;

                    attackState.SetGrounded(true);

                    break;
                }
            }
        }

        private void OnCollisionExit2D(
            Collision2D collision)
        {
            isGrounded = false;

            attackState.SetGrounded(false);
        }

        public void SetDead()
        {
            if (isDead)
                return;

            isDead = true;

            // Boss ölüm sesi
            if (bossAudio != null)
            {
                bossAudio.PlayDeath();
            }

            stateMachine.ChangeState(
                patrolState
            );

            rb.linearVelocity = Vector2.zero;

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );

                animator.SetTrigger(
                    "Die"
                );
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}