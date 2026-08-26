using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy1 : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();

            stateMachine.Initialize(
                new PatrolState()
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

                animator.SetFloat(
                    "Speed",
                    0f
                );
            }
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
                    animator.SetFloat(
                        "Speed",
                        0f
                    );
                }
                else
                {
                    animator.SetFloat(
                        "Speed",
                        1f
                    );
                }
            }
            else
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );
            }
        }

        public override void ResetToStart()
        {
            base.ResetToStart();

            stateMachine.Initialize(
                new PatrolState()
            );

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );
            }
        }
    }
}