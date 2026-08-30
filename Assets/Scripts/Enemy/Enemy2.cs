using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2 : EnemyBase
    {
        [Header("Patrol Settings")]

        // Enemy'nin başlangıç konumunun sağında ve solunda
        // ne kadar devriye gezeceğini belirler.
        [SerializeField] private float patrolDistance = 3f;

        // Enemy'nin oyun başladığındaki X konumu.
        private float patrolStartX;

        protected override void Awake()
        {
            base.Awake();

            // Başlangıç X konumunu kaydet.
            patrolStartX =
                transform.position.x;

            // Enemy 2'nin kendi patrol sistemini başlat.
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

                // Attack sırasında oyuncuya bak.
                FacePlayer();

                if (animator != null)
                {
                    animator.SetFloat(
                        "Speed",
                        0f
                    );
                }
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

                if (chaseState != null &&
                    chaseState.PlayerTooHigh)
                {
                    if (animator != null)
                    {
                        animator.SetFloat(
                            "Speed",
                            0f
                        );
                    }
                }
                else
                {
                    // Chase sırasında oyuncuya bak.
                    FacePlayer();

                    if (animator != null)
                    {
                        animator.SetFloat(
                            "Speed",
                            1f
                        );
                    }
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

        // Enemy2PatrolState tarafından çağrılır.
        // Patrol sırasında Enemy'nin hareket yönüne göre
        // Visual'ı çevirir.
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