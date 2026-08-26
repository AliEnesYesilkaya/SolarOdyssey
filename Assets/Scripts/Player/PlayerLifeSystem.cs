using SolarOdyssey.UI;
using SolarOdyssey.Combat;
using SolarOdyssey.Core;
using UnityEngine;

namespace SolarOdyssey.Player
{
    public class PlayerLifeSystem : MonoBehaviour
    {
        [Header("Lives")]
        [SerializeField] private int maxLives = 5;
        [SerializeField] private int currentLives = 5;

        [Header("Life Regeneration")]
        [SerializeField] private float lifeRegenerationTime = 900f;

        private float regenerationTimer;

        private LifeUI lifeUI;
        private LifeTimerUI lifeTimerUI;
        private LifePurchaseUI lifePurchaseUI;
        private CoinUI coinUI;
        private Health playerHealth;

        public int CurrentLives => currentLives;
        public int MaxLives => maxLives;

        private void Awake()
        {
            lifeUI =
                FindFirstObjectByType<LifeUI>();

            lifeTimerUI =
                FindFirstObjectByType<LifeTimerUI>(
                    FindObjectsInactive.Include
                );

            lifePurchaseUI =
                FindFirstObjectByType<LifePurchaseUI>(
                    FindObjectsInactive.Include
                );

            coinUI =
                FindFirstObjectByType<CoinUI>();

            playerHealth =
                GetComponent<Health>();

            regenerationTimer =
                lifeRegenerationTime;

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            if (lifeTimerUI != null)
            {
                lifeTimerUI.Hide();
            }

            if (lifePurchaseUI != null)
            {
                lifePurchaseUI.Hide();
            }
        }

        private void Update()
        {
            // 5 kalp varsa timer çalışmaz.
            if (currentLives >= maxLives)
                return;

            // Oyun durmuş olsa bile gerçek zaman ilerlesin.
            regenerationTimer -=
                Time.unscaledDeltaTime;

            if (regenerationTimer <= 0f)
            {
                AddLife();

                if (currentLives < maxLives)
                {
                    regenerationTimer =
                        lifeRegenerationTime;
                }
            }

            if (lifeTimerUI != null)
            {
                lifeTimerUI.UpdateTimer(
                    regenerationTimer
                );
            }
        }

        public void LoseLife()
        {
            if (currentLives <= 0)
                return;

            currentLives--;

            Debug.Log(
                "Oyuncu bir can kaybetti. Kalan can: " +
                currentLives
            );

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            // İlk kalp kaybedildiğinde timer başlar.
            if (currentLives == maxLives - 1)
            {
                regenerationTimer =
                    lifeRegenerationTime;

                if (lifeTimerUI != null)
                {
                    lifeTimerUI.Show();

                    lifeTimerUI.UpdateTimer(
                        regenerationTimer
                    );
                }
            }

            // Bütün kalpler bittiyse
            // satın alma ekranını aç ve oyunu durdur.
            if (currentLives <= 0)
            {
                Debug.Log(
                    "Oyuncunun bütün canları bitti!"
                );

                if (lifePurchaseUI != null)
                {
                    lifePurchaseUI.Show();

                    Time.timeScale = 0f;
                }
                else
                {
                    Debug.LogError(
                        "LifePurchaseUI bulunamadı!"
                    );
                }
            }
        }

        private void AddLife()
        {
            if (currentLives >= maxLives)
                return;

            currentLives++;

            Debug.Log(
                "Bir can yenilendi. Can: " +
                currentLives +
                "/" +
                maxLives
            );

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            // Eğer 0 kalpten timer sayesinde
            // tekrar 1 kalbe çıktıysak oyuncuyu
            // normal respawn sistemiyle geri getir.
            if (currentLives == 1 &&
                Time.timeScale == 0f)
            {
                if (playerHealth != null)
                {
                    // ÖNEMLİ:
                    // Sadece oyuncuyu değil,
                    // dünyayı da resetle.
                    if (RespawnManager.Instance != null)
                    {
                        RespawnManager.Instance.RespawnPlayer(
                            playerHealth
                        );
                    }
                    else
                    {
                        playerHealth.RespawnAtCheckpoint();
                    }
                }

                // Satın alma ekranını kapat.
                if (lifePurchaseUI != null)
                {
                    lifePurchaseUI.Hide();
                }

                // Oyunu devam ettir.
                Time.timeScale = 1f;
            }

            // 5 kalbe ulaştıysak timerı kapat.
            if (currentLives >= maxLives)
            {
                regenerationTimer =
                    lifeRegenerationTime;

                if (lifeTimerUI != null)
                {
                    lifeTimerUI.Hide();
                }

                return;
            }

            // Sonraki kalp için yeni süre başlat.
            regenerationTimer =
                lifeRegenerationTime;

            if (lifeTimerUI != null)
            {
                lifeTimerUI.Show();

                lifeTimerUI.UpdateTimer(
                    regenerationTimer
                );
            }
        }

        public bool BuyOneLife()
        {
            if (currentLives >= maxLives)
                return false;

            if (coinUI == null)
                return false;

            if (!coinUI.SpendGold(100))
                return false;

            AddLife();

            return true;
        }

        public bool BuyFiveLives()
        {
            if (currentLives >= maxLives)
                return false;

            if (coinUI == null)
                return false;

            if (!coinUI.SpendGold(300))
                return false;

            currentLives =
                maxLives;

            regenerationTimer =
                lifeRegenerationTime;

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            if (lifeTimerUI != null)
            {
                lifeTimerUI.Hide();
            }

            return true;
        }
    }
}