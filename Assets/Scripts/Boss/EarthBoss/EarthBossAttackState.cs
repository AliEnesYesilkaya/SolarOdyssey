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

        private float jumpForce;
        private float attackRange;
        private int attackDamage;

        private bool isGrounded;
        private bool attacking;

        public EarthBossAttackState(
            Rigidbody2D rb,
            Transform boss,
            Transform player,
            Animator animator,
            PlayerMovement playerMovement,
            float jumpForce,
            float attackRange,
            int attackDamage)
        {
            this.rb = rb;
            this.boss = boss;
            this.player = player;
            this.animator = animator;
            this.playerMovement = playerMovement;

            this.jumpForce = jumpForce;
            this.attackRange = attackRange;
            this.attackDamage = attackDamage;
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

            rb.linearVelocity = new Vector2(
                0f,
                jumpForce
            );

            boss.GetComponent<MonoBehaviour>()
                .StartCoroutine(WaitForLanding());
        }

        private IEnumerator WaitForLanding()
        {
            // Boss'un gerçekten havaya çıkmasını bekle.
            yield return new WaitForSeconds(0.2f);

            // Yere inmesini bekle.
            while (!isGrounded)
            {
                yield return null;
            }

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

            float horizontalDistance =
                Mathf.Abs(
                    player.position.x -
                    boss.position.x
                );

            // 10 birim dışındaysa hasar yok.
            if (horizontalDistance > attackRange)
                return;

            // Oyuncu havadaysa hasar yok.
            if (!playerMovement.IsGrounded)
                return;

            Health health =
                player.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(attackDamage);

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