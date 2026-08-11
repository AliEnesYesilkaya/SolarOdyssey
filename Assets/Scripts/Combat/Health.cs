using SolarOdyssey.Core;
using SolarOdyssey.Player;
using UnityEngine;
using SolarOdyssey.Player;

namespace SolarOdyssey.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int currentHealth;
        private Animator animator;
        private PlayerController playerController;
        private PlayerMovement playerMovement;
        private bool isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
            animator = GetComponentInChildren<Animator>();
            playerController = GetComponent<PlayerController>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            Debug.Log(gameObject.name + " Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            Debug.Log(gameObject.name + " Dead!");

            if (animator != null)
            {
                animator.SetTrigger("Dead");
            }

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            Invoke(nameof(Respawn), 0.7f);
        }

        private void Respawn()
        {
            if (Checkpoint.HasCheckpoint)
            {
                transform.position = Checkpoint.RespawnPosition;
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

            Debug.Log(gameObject.name + " Respawned! Health: " + currentHealth);
        }
    }
}