using SolarOdyssey.Core;
using SolarOdyssey.Player;
using SolarOdyssey.Enemy;
using UnityEngine;

namespace SolarOdyssey.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int currentHealth;
        private Animator animator;

        private PlayerController playerController;
        private PlayerMovement playerMovement;
        private EnemyBase enemyBase;

        private bool isDead;


        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        private void Awake()
        {
            currentHealth = maxHealth;

            // Child içerisindeki Animator'u bul
            animator = GetComponentInChildren<Animator>();

            // Eðer bu obje Player ise bunlar bulunacak
            playerController = GetComponent<PlayerController>();
            playerMovement = GetComponent<PlayerMovement>();

            // Eðer bu obje Enemy ise EnemyBase bulunacak
            enemyBase = GetComponent<EnemyBase>();
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            Debug.Log(
                gameObject.name +
                " Can: " +
                currentHealth
            );

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (isDead)
                return;

            currentHealth += amount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Debug.Log(
                gameObject.name +
                " Can: " +
                currentHealth
            );
        }

        public bool IsFullHealth()
        {
            return currentHealth >= maxHealth;
        }

        private void Die()
        {
            isDead = true;

            Debug.Log(gameObject.name + " Dead!");

            // Ölme animasyonunu baþlat
            if (animator != null)
            {
                animator.SetTrigger("Dead");
            }

            // -------------------------
            // PLAYER ÖLÜMÜ
            // -------------------------

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // -------------------------
            // ENEMY ÖLÜMÜ
            // -------------------------

            if (enemyBase != null)
            {
                // Enemy artýk AI çalýþtýrmasýn
                enemyBase.enabled = false;

                // Ölüm animasyonu oynadýktan sonra yok et
                Invoke(nameof(DestroyEnemy), 0.7f);

                return;
            }

            // -------------------------
            // PLAYER RESPAWN
            // -------------------------

            Invoke(nameof(Respawn), 0.7f);
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void Respawn()
        {
            if (Checkpoint.HasCheckpoint)
            {
                transform.position =
                    Checkpoint.RespawnPosition;
            }

            currentHealth = maxHealth;
            isDead = false;

            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            if (playerController != null)
            {
                playerController.enabled = true;
            }

            Debug.Log(
                gameObject.name +
                " Respawned! Health: " +
                currentHealth
            );
        }
    }
}