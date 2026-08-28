using SolarOdyssey.Core;
using SolarOdyssey.Player;
using SolarOdyssey.Enemy;
using UnityEngine;

namespace SolarOdyssey.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        // Boss öldüðünde oynatýlacak efekt.
        [SerializeField] private GameObject bossDeathEffect;

        private int currentHealth;

        private Animator animator;
        private PlayerAudio playerAudio;
        private PlayerController playerController;
        private PlayerMovement playerMovement;
        private PlayerLifeSystem playerLifeSystem;

        // Oyuncu hasar alma görsel efekti
        private PlayerDamageEffect playerDamageEffect;

        private EnemyBase enemyBase;
        private EarthBoss earthBoss;

        private bool isDead;

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;

        private void Awake()
        {
            animator =
                GetComponentInChildren<Animator>();

            playerAudio =
                GetComponent<PlayerAudio>();

            playerController =
                GetComponent<PlayerController>();

            playerMovement =
                GetComponent<PlayerMovement>();

            playerLifeSystem =
                GetComponent<PlayerLifeSystem>();

            // PlayerDamageEffect Player üzerinde varsa bulur.
            playerDamageEffect =
                GetComponent<PlayerDamageEffect>();

            enemyBase =
                GetComponent<EnemyBase>();

            earthBoss =
                GetComponent<EarthBoss>();

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

                // Oyuncu hasar alma sesi
                if (playerAudio != null)
                {
                    playerAudio.PlayHurt();
                }

                // Oyuncu hasar alma görsel efekti
                if (playerDamageEffect != null)
                {
                    playerDamageEffect.PlayDamageEffect();
                }
            }

            // Ölüm
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakePlayerAttackDamage(
            int normalDamage,
            int bossDamage)
        {
            if (isDead)
                return;

            int damage = normalDamage;

            if (earthBoss != null)
            {
                damage = bossDamage;
            }

            currentHealth -= damage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            Debug.Log(
                gameObject.name +
                " oyuncu saldýrýsýndan " +
                damage +
                " hasar aldý."
            );

            if (currentHealth > 0)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Hurt");
                }
            }

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

            if (playerAudio != null)
            {
                playerAudio.PlayDeath();
            }

            Debug.Log(
                gameObject.name +
                " Dead!"
            );

            if (animator != null)
            {
                animator.SetTrigger("Dead");
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

                // Boss öldüðünde havai fiþek efektini oluþtur.
                if (bossDeathEffect != null)
                {
                    Instantiate(
                        bossDeathEffect,
                        transform.position,
                        Quaternion.identity
                    );
                }

                Invoke(
                    nameof(DestroyCharacter),
                    1.2f
                );

                return;
            }

            // NORMAL ENEMY
            if (enemyBase != null)
            {
                enemyBase.PlayDeath();
                enemyBase.enabled = false;

                // Enemy'i yok etmiyoruz.
                // Dünya resetlendiðinde tekrar aktif edeceðiz.
                Invoke(
                    nameof(DisableEnemy),
                    0.7f
                );

                return;
            }

            // PLAYER LIFE SYSTEM
            if (playerLifeSystem != null)
            {
                // Oyuncunun kalbini azalt.
                playerLifeSystem.LoseLife();

                // Hâlâ kalp varsa checkpoint'e dön.
                if (playerLifeSystem.CurrentLives > 0)
                {
                    Invoke(
                        nameof(Respawn),
                        0.7f
                    );
                }

                // Bütün kalpler bittiyse oyunu durdur.
                if (playerLifeSystem.CurrentLives <= 0)
                {
                    Debug.Log(
                        "Bütün kalpler bitti. Oyun durduruldu."
                    );

                    Time.timeScale = 0f;
                }

                return;
            }

            // Güvenlik
            Invoke(
                nameof(Respawn),
                0.7f
            );
        }

        private void DisableEnemy()
        {
            gameObject.SetActive(false);
        }

        private void DestroyCharacter()
        {
            Destroy(gameObject);
        }

        private void Respawn()
        {
            if (RespawnManager.Instance != null)
            {
                // Tüm karakterleri RespawnManager'a göre spawn et.
                RespawnManager.Instance.RespawnPlayer(this);
                return;
            }

            RespawnAtCheckpoint();
        }

        public void RespawnAtCheckpoint()
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

        // Enemy dünyaya geri döndüðünde
        // Health sistemini tamamen baþlangýç haline getirir.
        public void ResetHealth()
        {
            currentHealth = maxHealth;
            isDead = false;

            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }
        }
    }
}