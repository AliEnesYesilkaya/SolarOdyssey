using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2 : EnemyBase
    {
        [Header("Patrol Settings")]

        [SerializeField] private float patrolDistance = 3f;

        private float patrolStartX;

        protected override void Awake()
        {
            base.Awake();

            patrolStartX =
                transform.position.x;

            stateMachine.Initialize(
                new Enemy2PatrolState(
                    rb,
                    patrolStartX,
                    patrolDistance,
                    moveSpeed,
                    FaceDirection
                )
            );
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;

            float distance =
                Vector2.Distance(
                    transform.position,
                    player.position
                );

            // =========================================
            // ATTACK
            // =========================================

            if (distance <= attackRange)
            {
                if (stateMachine.CurrentState
                    is not AttackState)
                {
                    stateMachine.ChangeState(
                        new AttackState(
                            transform,
                            player,
                            attackRange,
                            attackDamage
                        )
                    );
                }

                FacePlayer();

                if (animator != null)
                {
                    animator.SetFloat(
                        "Speed",
                        0f
                    );
                }

                return;
            }

            // =========================================
            // CHASE
            // =========================================

            else if (distance <= detectionRange)
            {
                if (stateMachine.CurrentState
                    is not ChaseState)
                {
                    stateMachine.ChangeState(
                        new ChaseState(
                            rb,
                            player,
                            moveSpeed,
                            animator
                        )
                    );
                }

                ChaseState chaseState =
                    stateMachine.CurrentState
                    as ChaseState;

                // =====================================
                // OYUNCU ÇOK YUKARIDA
                // =====================================

                if (chaseState != null &&
                    chaseState.PlayerTooHigh)
                {
                    // Enemy2 oyuncuya doğru yatay
                    // hareket ETMESİN.
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

                    return;
                }

                // =====================================
                // NORMAL CHASE
                // =====================================

                FacePlayer();

                if (animator != null)
                {
                    animator.SetFloat(
                        "Speed",
                        1f
                    );
                }
            }

            // =========================================
            // PATROL
            // =========================================

            else
            {
                if (stateMachine.CurrentState
                    is not Enemy2PatrolState)
                {
                    stateMachine.ChangeState(
                        new Enemy2PatrolState(
                            rb,
                            patrolStartX,
                            patrolDistance,
                            moveSpeed,
                            FaceDirection
                        )
                    );
                }

                if (animator != null)
                {
                    animator.SetFloat(
                        "Speed",
                        1f
                    );
                }
            }
        }

        // =============================================
        // PATROL YÖNÜ
        // =============================================

        private void FaceDirection(float direction)
        {
            Transform visual =
                transform.Find("Visual");

            if (visual == null)
                return;

            if (direction == 0f)
                return;

            Vector3 scale =
                visual.localScale;

            scale.x =
                Mathf.Abs(scale.x) *
                Mathf.Sign(direction);

            visual.localScale =
                scale;
        }

        // =============================================
        // RESET
        // =============================================

        public override void ResetToStart()
        {
            base.ResetToStart();

            patrolStartX =
                transform.position.x;

            stateMachine.Initialize(
                new Enemy2PatrolState(
                    rb,
                    patrolStartX,
                    patrolDistance,
                    moveSpeed,
                    FaceDirection
                )
            );

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    1f
                );
            }
        }
    }
}