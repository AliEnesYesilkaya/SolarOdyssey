using SolarOdyssey.Core;
using SolarOdyssey.Player;
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
        private bool isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
            animator = GetComponentInChildren<Animator>(); //player childýnýn animatörünü bul.
            playerController = GetComponent<PlayerController>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            Debug.Log(gameObject.name + " Can: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (isDead)
                return;

            currentHealth += amount;//iyileþme miktarý ekle

            if (currentHealth > maxHealth)//maxý geçerse maxa eþitles
            {
                currentHealth = maxHealth;
            }

            Debug.Log(gameObject.name + " Can: " + currentHealth);
        }
        public bool IsFullHealth()
{
    return currentHealth >= maxHealth;
}

        private void Die()
        {
            isDead = true;

            Debug.Log(gameObject.name + " Dead!");

            if (animator != null) // ölme animasyonunu baþlat 
            {
                animator.SetTrigger("Dead");
            }

            if (playerController != null) //öldüðü süre hareket ve kontroller durdur 
            {
                playerController.enabled = false;
            }

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            Invoke(nameof(Respawn), 0.7f);// 0.7 saniye sonra tekrar doð
        }

        private void Respawn()
        {
            if (Checkpoint.HasCheckpoint)
            {
                transform.position = Checkpoint.RespawnPosition; //oyuncuyu checkpoint noktasýna spawnla 
            }

            currentHealth = maxHealth; //caný maxla,ölü isaretini kaldýr 
            isDead = false;

            if (animator != null) // animasyonu varsayýlana sýfýrla 
            {
                animator.Rebind();
                animator.Update(0f);
            }

            if (playerMovement != null) // kontroller ve hareket tekrardan aktif 
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