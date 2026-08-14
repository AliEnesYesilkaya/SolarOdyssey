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
            //mesafe hesabı 
            float distance = Vector2.Distance(transform.position, player.position);
               
            // Oyuncu algılama alanına girdiyse takibe'e geç
            if (distance <= detectionRange)
            {
                if (stateMachine.CurrentState is not ChaseState)
                {
                    stateMachine.ChangeState(
                        new ChaseState(rb, player, moveSpeed));  
                }
            }
            // Oyuncu algılama alanından çıktıysa devriyeye dön
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