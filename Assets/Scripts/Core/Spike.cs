using SolarOdyssey.Audio;
using SolarOdyssey.Combat;
using SolarOdyssey.Enemy;
using UnityEngine;

namespace SolarOdyssey.Core
{
    public class Spike : MonoBehaviour
    {
        [Header("Player Damage")]
        [SerializeField] private int damage = 20;
        [SerializeField] private float damageInterval = 1f;

        [Header("Enemy Damage")]
        [SerializeField] private int enemyDamage = 1;
        [SerializeField] private float enemyDamageInterval = 1f;

        private bool isInWater;
        private float damageTimer;

        private Health playerHealth;

        private bool enemyInWater;
        private float enemyDamageTimer;

        private Health enemyHealth;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // =========================================
            // PLAYER
            // =========================================

            if (other.CompareTag("Player"))
            {
                playerHealth =
                    other.GetComponent<Health>();

                if (playerHealth == null)
                    return;

                isInWater = true;

                // Ýlk hasar
                playerHealth.TakeDamage(damage);

                damageTimer = damageInterval;

                // Suya düþme sesi
                if (EnvironmentAudio.Instance != null)
                {
                    EnvironmentAudio.Instance.PlayWaterSplash();
                }

                return;
            }

            // =========================================
            // ENEMY
            // =========================================

            EnemyBase enemy =
                other.GetComponentInParent<EnemyBase>();

            if (enemy == null)
                return;

            enemyHealth =
                enemy.GetComponent<Health>();

            if (enemyHealth == null)
                return;

            enemyInWater = true;

            // Enemy ilk girdiðinde hasar
            enemyHealth.TakeDamage(enemyDamage);

            enemyDamageTimer =
                enemyDamageInterval;
        }

        private void Update()
        {
            // =========================================
            // PLAYER SUDA
            // =========================================

            if (isInWater)
            {
                if (playerHealth == null)
                    return;

                damageTimer -= Time.deltaTime;

                if (damageTimer <= 0f)
                {
                    playerHealth.TakeDamage(damage);

                    damageTimer = damageInterval;
                }
            }

            // =========================================
            // ENEMY SUDA
            // =========================================

            if (enemyInWater)
            {
                if (enemyHealth == null)
                    return;

                enemyDamageTimer -= Time.deltaTime;

                if (enemyDamageTimer <= 0f)
                {
                    enemyHealth.TakeDamage(enemyDamage);

                    enemyDamageTimer =
                        enemyDamageInterval;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // =========================================
            // PLAYER
            // =========================================

            if (other.CompareTag("Player"))
            {
                isInWater = false;
                playerHealth = null;
                damageTimer = 0f;

                return;
            }

            // =========================================
            // ENEMY
            // =========================================

            EnemyBase enemy =
                other.GetComponentInParent<EnemyBase>();

            if (enemy == null)
                return;

            enemyInWater = false;
            enemyHealth = null;
            enemyDamageTimer = 0f;
        }
    }
}