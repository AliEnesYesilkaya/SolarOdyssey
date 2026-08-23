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
        private EarthBoss earthBoss;

        private bool isDead;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            playerController = GetComponent<PlayerController>();
            playerMovement = GetComponent<PlayerMovement>();
            enemyBase = GetComponent<EnemyBase>();
            earthBoss = GetComponent<EarthBoss>();

            // BOSS
            if (earthBoss != null)
            {
                maxHealth = 200;
            }

            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            // BOSS HER ZAMAN 5 HASAR ALIR
            if (earthBoss != null)
            {
                damage = 5;
            }

            currentHealth -= damage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            Debug.Log(
                gameObject.name +
                " Can: " +
                currentHealth +
                "/" +
                maxHealth
            );

            // Hurt
            if (currentHealth > 0)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Hurt");
                }
            }

            // Ölüm
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
        }

        public bool IsFullHealth()
        {
            return currentHealth >= maxHealth;
        }

        private void Die()
        {
            isDead = true;

            Debug.Log(gameObject.name + " Dead!");

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            // PLAYER
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // BOSS
            if (earthBoss != null)
            {
                earthBoss.SetDead();

                Invoke(
                    nameof(DestroyCharacter),
                    1.2f
                );

                return;
            }

            // NORMAL ENEMY
            if (enemyBase != null)
            {
                enemyBase.enabled = false;

                Invoke(
                    nameof(DestroyCharacter),
                    0.7f
                );

                return;
            }

            // PLAYER RESPAWN
            Invoke(
                nameof(Respawn),
                0.7f
            );
        }

        private void DestroyCharacter()
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
        }
    }
}