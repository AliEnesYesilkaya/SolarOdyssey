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

            currentLives =
                Mathf.Clamp(
                    currentLives,
                    0,
                    maxLives
                );

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
            if (currentLives >= maxLives)
            {
                if (lifeTimerUI != null)
                {
                    lifeTimerUI.Hide();
                }

                return;
            }

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

            // İlk can kaybında timer başlar.
            if (currentLives == maxLives - 1)
            {
                regenerationTimer =
                    lifeRegenerationTime;
            }

            if (currentLives < maxLives &&
                lifeTimerUI != null)
            {
                lifeTimerUI.Show();

                lifeTimerUI.UpdateTimer(
                    regenerationTimer
                );
            }

            // Bütün canlar bittiyse satın alma ekranı açılır.
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

            // Canlar tamamen dolduysa timer kapanır.
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

            // Sonraki can için timer yeniden başlar.
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

            if (!coinUI.SpendGold(500))
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

            // 5 can satın alındığında oyuncuyu
            // tekrar canlı hale getir.
            RespawnPlayer();

            return true;
        }
    }
}