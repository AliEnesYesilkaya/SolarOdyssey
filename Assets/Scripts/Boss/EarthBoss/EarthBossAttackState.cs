using System.Collections;
using SolarOdyssey.Combat;
using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EarthBossAttackState : IState
    {
        private Rigidbody2D rb;
        private Transform boss;
        private Transform player;

        private Animator animator;
        private PlayerMovement playerMovement;
        private EarthBossAudio bossAudio;

        private float jumpForce;
        private float attackRange;
        private int attackDamage;

        private bool isGrounded;
        private bool attacking;

      
        private float knockbackForce = 5f;

        public EarthBossAttackState(
            Rigidbody2D rb,
            Transform boss,
            Transform player,
            Animator animator,
            PlayerMovement playerMovement,
            float jumpForce,
            float attackRange,
            int attackDamage,
            EarthBossAudio bossAudio)
        {
            this.rb = rb;
            this.boss = boss;
            this.player = player;
            this.animator = animator;
            this.playerMovement = playerMovement;
            this.jumpForce = jumpForce;
            this.attackRange = attackRange;
            this.attackDamage = attackDamage;
            this.bossAudio = bossAudio;
        }

        public void Enter()
        {
            attacking = true;

            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
                animator.SetTrigger("Attack");
            }

            Debug.Log("Earth Boss Attack başladı");

            if (bossAudio != null)
            {
                bossAudio.PlayJump();
            }

            rb.linearVelocity = new Vector2(
                0f,
                jumpForce
            );

            boss.GetComponent<MonoBehaviour>()
                .StartCoroutine(WaitForLanding());
        }

        private IEnumerator WaitForLanding()
        {
            // Bossun gerçekten havaya çıkmasını bekle.
            yield return new WaitForSeconds(0.2f);

            // Yere inmesini bekle.
            while (!isGrounded)
            {
                yield return null;
            }

            // Yere çarpma sesi.
            if (bossAudio != null)
            {
                bossAudio.PlaySlam();
            }

            // Boss yere indiği anda slam hasarı.
            DealSlamDamage();

            yield return new WaitForSeconds(0.3f);

            attacking = false;
        }

        private void DealSlamDamage()
        {
            if (player == null)
                return;

            if (playerMovement == null)
                return;

            // Oyuncu havadaysa hasar yok.
            if (!playerMovement.IsGrounded)
                return;

            float horizontalDistance =
                Mathf.Abs(
                    player.position.x -
                    boss.position.x
                );

            // Oyuncu bossun saldırı alanındaysa hasar.
            if (horizontalDistance > attackRange)
                return;

            Health health =
                player.GetComponent<Health>();

            if (health != null)
            {
                // Hasar ver.
                health.TakeDamage(attackDamage);

               
                Rigidbody2D playerRb =
                    player.GetComponent<Rigidbody2D>();

                if (playerRb != null)
                {
                    // Oyuncu bossun solundaysa sola,
                    // sağındaysa sağa it.
                    float pushDirection =
                        player.position.x >= boss.position.x
                            ? 1f
                            : -1f;
                    //fizik hızına bağlı sabit itme 
                    playerRb.linearVelocity =
                        new Vector2(
                            pushDirection * knockbackForce,
                            playerRb.linearVelocity.y
                        );
                }

                Debug.Log(
                    "Earth Boss slam: " +
                    attackDamage +
                    " hasar."
                );
            }
        }

        public void SetGrounded(bool grounded)
        {
            isGrounded = grounded;
        }

        public bool IsAttacking()
        {
            return attacking;
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            Debug.Log("Earth Boss Attack bitti");
        }
    }
}