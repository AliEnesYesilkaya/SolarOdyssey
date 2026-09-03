using System;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy2PatrolState : IState
    {
        private Rigidbody2D rb;

        // Enemy'nin başlangıç X konumu.
        private float startX;

        // Başlangıç konumundan sağa/sola
        // ne kadar gidebileceği.
        private float patrolDistance;

        private float moveSpeed;

        // Devriyenin sınırları.
        private float leftLimit;
        private float rightLimit;

        // 1 = sağ
        // -1 = sol
        private int direction;

        // Enemy2'nin Visual'ını hareket yönüne
        // göre çevirmek için kullanılacak fonksiyon.
        private Action<float> faceDirection;


        public Enemy2PatrolState(
            Rigidbody2D rb,
            float startX,
            float patrolDistance,
            float moveSpeed,
            Action<float> faceDirection)
        {
            this.rb = rb;
            this.startX = startX;
            this.patrolDistance = patrolDistance;
            this.moveSpeed = moveSpeed;
            this.faceDirection = faceDirection;

            // Sol sınır.
            leftLimit =
                startX - patrolDistance;

            // Sağ sınır.
            rightLimit =
                startX + patrolDistance;

            // İlk olarak sağa git.
            direction = 1;
        }


        public void Enter()
        {
            Debug.Log(
                "Enemy2 Patrol başladı"
            );

            // Başlangıç yönüne göre Visual'ı çevir.
            faceDirection?.Invoke(direction);
        }


        public void Tick()
        {
            if (rb == null)
                return;

            // =========================================
            // SAĞ SINIRA ULAŞTI MI?
            // =========================================

            if (direction > 0 &&
                rb.position.x >= rightLimit)
            {
                direction = -1;

                // Yön değişti → Visual'ı sola çevir.
                faceDirection?.Invoke(direction);
            }

            // =========================================
            // SOL SINIRA ULAŞTI MI?
            // =========================================

            else if (direction < 0 &&
                     rb.position.x <= leftLimit)
            {
                direction = 1;

                // Yön değişti → Visual'ı sağa çevir.
                faceDirection?.Invoke(direction);
            }

            // =========================================
            // HAREKET
            // =========================================

            rb.linearVelocity =
                new Vector2(
                    direction * moveSpeed,
                    rb.linearVelocity.y
                );

            // Hareket edilen yöne göre Visual'ı sürekli düzelt.
            faceDirection?.Invoke(direction);
        }


        public void Exit()
        {
            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

            Debug.Log(
                "Enemy2 Patrol bitti"
            );
        }
    }
}