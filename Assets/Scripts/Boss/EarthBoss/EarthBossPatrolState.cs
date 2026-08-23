using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EarthBossPatrolState : IState
    {
        private Rigidbody2D rb;
        private Transform boss;
        private Transform pointA;
        private Transform pointB;
        private float moveSpeed;

        private Transform targetPoint;

        public EarthBossPatrolState(
            Rigidbody2D rb,
            Transform boss,
            Transform pointA,
            Transform pointB,
            float moveSpeed)
        {
            this.rb = rb;
            this.boss = boss;
            this.pointA = pointA;
            this.pointB = pointB;
            this.moveSpeed = moveSpeed;

            targetPoint = pointB;
        }

        public void Enter()
        {
            Debug.Log("Earth Boss Patrol başladı");
        }

        public void Tick()
        {
            if (pointA == null || pointB == null)
                return;

            float distance =
                Mathf.Abs(
                    rb.position.x -
                    targetPoint.position.x
                );

            // Hedef noktaya ulaştıysa diğer noktaya geç.
            if (distance < 0.1f)
            {
                targetPoint =
                    targetPoint == pointA
                    ? pointB
                    : pointA;
            }

            float direction =
                Mathf.Sign(
                    targetPoint.position.x -
                    rb.position.x
                );

            rb.linearVelocity = new Vector2(
                direction * moveSpeed,
                rb.linearVelocity.y
            );
        }

        public void Exit()
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            Debug.Log("Earth Boss Patrol bitti");
        }
    }
}