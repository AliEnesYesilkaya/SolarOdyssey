using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class ChaseState : IState
    {
        private Rigidbody2D rb;
        private Transform player;
        private float moveSpeed;

        public ChaseState(
            Rigidbody2D rb,
            Transform player,
            float moveSpeed)
        {
            this.rb = rb;
            this.player = player;
            this.moveSpeed = moveSpeed;
        }

        public void Enter()
        {
            Debug.Log("Chase State başladı");
        }

        public void Tick()
        {
            if (player == null)
                return;

            float dirX = Mathf.Sign(
                player.position.x - rb.position.x
            );

            // Sadece yatay hareketi AI kontrol eder.
            // Y hızına dokunmuyoruz.
            rb.linearVelocity = new Vector2(
                dirX * moveSpeed,
                rb.linearVelocity.y
            );
        }

        public void Exit()
        {
            // Chase bittiğinde yatay hareketi durdur.
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            Debug.Log("Chase State bitti");
        }
    }
}