using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.UI
{
    public class LifePurchaseUI : MonoBehaviour
    {
        private PlayerLifeSystem playerLifeSystem;

        private void Awake()
        {
            playerLifeSystem =
                FindFirstObjectByType<PlayerLifeSystem>();
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
                Time.timeScale = 1f;

                Hide();
            }
        }

        public void BuyFiveLives()
        {
            if (playerLifeSystem == null)
                return;

            if (playerLifeSystem.BuyFiveLives())
            {
                Time.timeScale = 1f;

                Hide();
            }
        }
    }
}