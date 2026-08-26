using SolarOdyssey.Player;
using SolarOdyssey.Combat;
using SolarOdyssey.Core;
using UnityEngine;

namespace SolarOdyssey.UI
{
    public class LifePurchaseUI : MonoBehaviour
    {
        private PlayerLifeSystem playerLifeSystem;
        private Health playerHealth;

        private void Awake()
        {
            playerLifeSystem =
                FindFirstObjectByType<PlayerLifeSystem>();

            playerHealth =
                FindFirstObjectByType<Health>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void BuyOneLife()
        {
            if (playerLifeSystem == null)
                return;

            if (playerLifeSystem.BuyOneLife())
            {
                // Can satın alındığında oyuncuyu
                // checkpoint'e gönder ve dünyayı resetle.
                if (playerHealth != null)
                {
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

                // Oyunu tekrar çalıştır.
                Time.timeScale = 1f;

                // Satın alma ekranını kapat.
                Hide();
            }
        }

        public void BuyFiveLives()
        {
            if (playerLifeSystem == null)
                return;

            if (playerLifeSystem.BuyFiveLives())
            {
                // 5 can satın alındığında oyuncuyu
                // checkpoint'e gönder ve dünyayı resetle.
                if (playerHealth != null)
                {
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

                // Oyunu tekrar çalıştır.
                Time.timeScale = 1f;

                // Satın alma ekranını kapat.
                Hide();
            }
        }
    }
}