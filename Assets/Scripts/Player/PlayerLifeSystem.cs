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

            // Daha önce kayıt yapılmışsa
            // kayıtlı can sayısını yükle.
            if (SaveSystem.Instance != null)
            {
                currentLives =
                    SaveSystem.Instance.LoadLives(
                        maxLives
                    );
            }
            else
            {
                currentLives =
                    Mathf.Clamp(
                        currentLives,
                        0,
                        maxLives
                    );
            }

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
            // Canlar tamamen doluysa
            // timer çalışmaz.
            if (currentLives >= maxLives)
            {
                if (lifeTimerUI != null)
                {
                    lifeTimerUI.Hide();
                }

                return;
            }

            // Oyun durmuş olsa bile
            // gerçek zaman üzerinden çalışır.
            regenerationTimer -=
                Time.unscaledDeltaTime;

            if (regenerationTimer <= 0f)
            {
                AddLife();
                return;
            }

            if (lifeTimerUI != null)
            {
                lifeTimerUI.Show();

                lifeTimerUI.UpdateTimer(
                    regenerationTimer
                );
            }
        }

        public void LoseLife()
        {
            if (currentLives <= 0)
                return;

            // Bir can kaybet.
            currentLives--;

            Debug.Log(
                "Oyuncu bir can kaybetti. Kalan can: " +
                currentLives
            );

            // Yeni can değerini kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveLives(
                    currentLives
                );
            }

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            // İlk can kaybında
            // 15 dakikalık timer başlar.
            if (currentLives == maxLives - 1)
            {
                regenerationTimer =
                    lifeRegenerationTime;
            }

            // Can maksimumdan düşük olduğu sürece
            // timer görünür.
            if (currentLives < maxLives &&
                lifeTimerUI != null)
            {
                lifeTimerUI.Show();

                lifeTimerUI.UpdateTimer(
                    regenerationTimer
                );
            }

            // Bütün canlar bittiyse
            // satın alma ekranını aç.
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

            // Bir can yenile.
            currentLives++;

            Debug.Log(
                "Bir can yenilendi. Can: " +
                currentLives +
                "/" +
                maxLives
            );

            // Yeni can değerini kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveLives(
                    currentLives
                );
            }

            if (lifeUI != null)
            {
                lifeUI.UpdateLives(
                    currentLives,
                    maxLives
                );
            }

            // 0 candan tekrar 1 cana çıktıysak
            // oyuncuyu ve dünyayı yeniden başlat.
            if (currentLives == 1 &&
                Time.timeScale == 0f)
            {
                RespawnPlayer();

                if (lifePurchaseUI != null)
                {
                    lifePurchaseUI.Hide();
                }

                Time.timeScale = 1f;
            }

            // Canlar tamamen dolduysa
            // timer kapanır.
            if (currentLives >= maxLives)
            {
                currentLives =
                    maxLives;

                regenerationTimer =
                    lifeRegenerationTime;

                if (lifeTimerUI != null)
                {
                    lifeTimerUI.Hide();
                }

                return;
            }

            // Sonraki can için
            // timer yeniden başlar.
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

        private void RespawnPlayer()
        {
            if (playerHealth == null)
                return;

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

        public bool BuyOneLife()
        {
            if (currentLives >= maxLives)
                return false;

            if (coinUI == null)
                return false;

            // 1 can = 150 altın.
            if (!coinUI.SpendGold(150))
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

            // 5 can = 500 altın.
            if (!coinUI.SpendGold(500))
                return false;

            // Canları tamamen doldur.
            currentLives =
                maxLives;

            // Can değerini kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveLives(
                    currentLives
                );
            }

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

            // 5 can satın alındığında
            // oyuncuyu tekrar canlı hale getir.
            RespawnPlayer();

            return true;
        }
    }
}