using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy1 : EnemyBase
    {
        protected override void Awake()//enemybase ı çalıştır ve ilk durum olarak devriye at 
        {
            base.Awake();

            stateMachine.Initialize(new PatrolState());
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;
            //mesafeyi hesapla oyuncu menzil içindeyse ve takip edilmiyorsa chase durumunu başlat 
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                if (stateMachine.CurrentState is not ChaseState)
                {
                    stateMachine.ChangeState(
                        new ChaseState(transform, player, moveSpeed)
                    );
                }
            }
        }
    }
}