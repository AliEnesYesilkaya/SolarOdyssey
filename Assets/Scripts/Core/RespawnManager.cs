using SolarOdyssey.Combat;
using SolarOdyssey.Enemy;
using SolarOdyssey;
using UnityEngine;

namespace SolarOdyssey.Core
{
    public class RespawnManager : MonoBehaviour
    {
        public static RespawnManager Instance { get; private set; }

        private GoldPickup[] goldPickups;
        private HeartPickup[] heartPickups;
        private EnemyBase[] enemies;
        private EarthBoss[] bosses;

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Bütün altın pickup'larını bul.
            goldPickups =
                FindObjectsByType<GoldPickup>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            // Bütün toplanabilir kalpleri bul.
            heartPickups =
                FindObjectsByType<HeartPickup>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            // Bütün normal düşmanları bul.
            enemies =
                FindObjectsByType<EnemyBase>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );

            // Bütün bossları bul.
            bosses =
                FindObjectsByType<EarthBoss>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                );
        }

        public void RespawnPlayer(
            Health playerHealth)
        {
            if (playerHealth == null)
                return;

            Debug.Log(
                "Dünya başlangıç durumuna döndürülüyor."
            );

            // --------------------------------------------
            // DÜNYAYI RESETLE
            // --------------------------------------------

            ResetGold();

            ResetHearts();

            ResetEnemies();

            ResetBossHealth();

            // --------------------------------------------
            // OYUNCUYU CHECKPOINT'E GÖNDER
            // --------------------------------------------

            playerHealth.RespawnAtCheckpoint();

            Debug.Log(
                "Respawn tamamlandı."
            );
        }

        private void ResetGold()
        {
            if (goldPickups == null)
                return;

            foreach (GoldPickup gold
                     in goldPickups)
            {
                if (gold == null)
                    continue;

                gold.Respawn();
            }
        }

        private void ResetHearts()
        {
            if (heartPickups == null)
                return;

            foreach (HeartPickup heart
                     in heartPickups)
            {
                if (heart == null)
                    continue;

                heart.Respawn();
            }
        }

        private void ResetEnemies()
        {
            if (enemies == null)
                return;

            foreach (EnemyBase enemy
                     in enemies)
            {
                if (enemy == null)
                    continue;

                enemy.ResetToStart();
            }
        }

        private void ResetBossHealth()
        {
            if (bosses == null)
                return;

            foreach (EarthBoss boss
                     in bosses)
            {
                if (boss == null)
                    continue;

                // Boss ölmediyse sadece canını doldur.
                Health health =
                    boss.GetComponent<Health>();

                if (health != null &&
                    !boss.IsDead())
                {
                    health.ResetHealth();
                }
            }
        }
    }
}