using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2 : EnemyBase
    {
        [Header("Patrol Points")]
        [SerializeField] private Transform patrolPointA;
        [SerializeField] private Transform patrolPointB;

        protected override void Awake()
        {
            base.Awake();

            stateMachine.Initialize( //state machine başlangıcı a b noktaları arası devriye 
                new Enemy2PatrolState(rb, patrolPointA, patrolPointB, moveSpeed)
                   );
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;

            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            // Saldırı menzilindeyse Attack
            if (distance <= attackRange)
            {
                if (stateMachine.CurrentState is not AttackState)
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
            }
            // Algılama menzilindeyse Chase
            else if (distance <= detectionRange)
            {
                if (stateMachine.CurrentState is not ChaseState)
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
            }
            // Algılama alanından çıktıysa Patrol
            else
            {
                if (stateMachine.CurrentState is not Enemy2PatrolState)
                {
                    stateMachine.ChangeState(
                        new Enemy2PatrolState(
                            rb,
                            patrolPointA,
                            patrolPointB,
                            moveSpeed
                        )
                    );
                }
            }
        }
    }
}