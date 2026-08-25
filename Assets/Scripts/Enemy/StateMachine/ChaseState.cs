using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class ChaseState : IState
    {
        private Rigidbody2D rb;
        private Transform player;
        private float moveSpeed;
        private Animator animator;

        private float jumpForce = 14f;
        private float jumpCooldown = 1.2f;
        private float jumpTimer;

        private float jumpHeightThreshold = 0.8f;
        private float maxJumpHeight = 4f;
        private float maxJumpHorizontalDistance = 8f;

        // Düşmanın oyuncunun zıplamasına tepki verme süresi
        private float jumpReactionDelay = 0.1f;
        private float jumpReactionTimer;

        private bool waitingForJump;

        public bool PlayerTooHigh { get; private set; }

        public ChaseState(
            Rigidbody2D rb,
            Transform player,
            float moveSpeed,
            Animator animator,
            float jumpForce = 14f)
        {
            this.rb = rb;
            this.player = player;
            this.moveSpeed = moveSpeed;
            this.animator = animator;
            this.jumpForce = jumpForce;
        }

        public void Enter()
        {
            jumpTimer = 0f;
            jumpReactionTimer = 0f;
            waitingForJump = false;
            PlayerTooHigh = false;

            Debug.Log("Chase State başladı");
        }

        public void Tick()
        {
            if (player == null)
                return;

            float verticalDistance =
                player.position.y - rb.position.y;

            float horizontalDistance =
                Mathf.Abs(
                    player.position.x - rb.position.x
                );

            jumpTimer -= Time.fixedDeltaTime;

            // -----------------------------------------
            // OYUNCU ULAŞILAMAYACAK KADAR YUKARIDA
            // -----------------------------------------

            if (verticalDistance > maxJumpHeight)
            {
                PlayerTooHigh = true;

                waitingForJump = false;
                jumpReactionTimer = 0f;

                rb.linearVelocity = new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

                return;
            }

            PlayerTooHigh = false;

            // -----------------------------------------
            // NORMAL YÜRÜME
            // -----------------------------------------

            float dirX = Mathf.Sign(
                player.position.x - rb.position.x
            );

            rb.linearVelocity = new Vector2(
                dirX * moveSpeed,
                rb.linearVelocity.y
            );

            // -----------------------------------------
            // ZIPLAMA
            // -----------------------------------------

            if (verticalDistance > jumpHeightThreshold &&
                verticalDistance <= maxJumpHeight &&
                horizontalDistance <= maxJumpHorizontalDistance)
            {
                // Oyuncu yukarı çıktığında bir kere bekleme başlat.
                if (!waitingForJump)
                {
                    waitingForJump = true;
                    jumpReactionTimer = jumpReactionDelay;
                }

                jumpReactionTimer -= Time.fixedDeltaTime;

                // Artık Y velocity kontrolü YOK.
                if (jumpTimer <= 0f &&
                    jumpReactionTimer <= 0f)
                {
                    rb.linearVelocity = new Vector2(
                        rb.linearVelocity.x,
                        jumpForce
                    );

                    jumpTimer = jumpCooldown;

                    waitingForJump = false;
                    jumpReactionTimer = 0f;

                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                    }
                }
            }
            else
            {
                waitingForJump = false;
                jumpReactionTimer = 0f;
            }
        }

        public void Exit()
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            waitingForJump = false;
            jumpReactionTimer = 0f;
            PlayerTooHigh = false;

            Debug.Log("Chase State bitti");
        }
    }
}