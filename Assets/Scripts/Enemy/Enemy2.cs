using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2 : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();

            // İlk durum: devriye
            stateMachine.Initialize(new PatrolState());
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;

            // Oyuncu ile düşman arasındaki mesafe
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            // Oyuncu algılama alanına girdiyse Chase'e geç
            if (distance <= detectionRange)
            {
                if (stateMachine.CurrentState is not ChaseState)
                {
                    stateMachine.ChangeState(
                        new ChaseState(
                            transform,
                            player,
                            moveSpeed
                        )
                    );
                }
            }
            // Oyuncu algılama alanından çıktıysa Patrol'a dön
            else
            {
                if (stateMachine.CurrentState is not PatrolState)
                {
                    stateMachine.ChangeState(
                        new PatrolState()
                    );
                }
            }
        }
    }
}