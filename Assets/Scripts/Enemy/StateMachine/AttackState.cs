using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class AttackState : IState
    {
        private Transform enemy;
        private Transform player;
        private float attackRange;
        private int attackDamage;
        //iki saldırı arası bekleme süresi
        private float attackCooldown = 1f;
        private float attackTimer;

        private Animator animator;

        public AttackState(Transform enemy, Transform player, float attackRange, int attackDamage)
        {
            this.enemy = enemy;
            this.player = player;
            this.attackRange = attackRange;
            this.attackDamage = attackDamage;

            animator = enemy.GetComponentInChildren<Animator>();
        }

        public void Enter()
        {
            attackTimer = 0f;//zamanlayıcıyı 0la

            if (animator != null)//animasyon trigger çalıştır 
            {
                animator.SetTrigger("Attack");
            }

            Debug.Log("Attack State başladı");
        }

        public void Tick()
        {
            if (player == null)
                return;
            //mesafe hesabı
            float distance = Vector2.Distance(
                enemy.position,
                player.position
            );
            
            if (distance > attackRange)
                return;
            //zamanlayıcıyı geri say
            attackTimer -= Time.fixedDeltaTime;

            if (attackTimer <= 0f)
            {
                // Sadece saldırı animasyonunu başlat.
                // Hasar Animation Event ile verilecek.
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                attackTimer = attackCooldown;
            }
        }

        public void Exit()
        {
            Debug.Log("Attack State bitti");
        }
    }
}