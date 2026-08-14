using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2PatrolState : IState
    {
        private Rigidbody2D rb;
        private Transform pointA;
        private Transform pointB;
        private float moveSpeed;

        private Transform targetPoint;

        public Enemy2PatrolState (Rigidbody2D rb, Transform pointA, Transform pointB, float moveSpeed)     
        {
            this.rb = rb;
            this.pointA = pointA;
            this.pointB = pointB;
            this.moveSpeed = moveSpeed;
            //ilk devriye atılacak hedef b noktası 
            targetPoint = pointB;
        }

        public void Enter()
        {
            Debug.Log("Enemy2 Patrol başladı");
        }

        public void Tick()
        { //devriye noktaları null ise işlemi durdur 
            if (pointA == null || pointB == null)
                return;
            //hedef nokta ve düşman konumları arası yön farkı 
            float dirX = Mathf.Sign(
                targetPoint.position.x - rb.position.x
            );

            // Sadece yatay hareket veriyoruz.
            // Y hızını Rigidbody2D ve gravity kontrol ediyor.
            rb.linearVelocity = new Vector2(
                dirX * moveSpeed,
                rb.linearVelocity.y
            );

            // Hedef noktaya ulaştıysa 
            if (Mathf.Abs(
                    rb.position.x - targetPoint.position.x
                ) < 0.05f)
            {//hedefi değiştir
                targetPoint =
                    targetPoint == pointA
                    ? pointB
                    : pointA;
            }
        }
        //devriyeden çıkarken hızı sıfırla
        public void Exit()
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            Debug.Log("Enemy2 Patrol bitti");
        }
    }
}