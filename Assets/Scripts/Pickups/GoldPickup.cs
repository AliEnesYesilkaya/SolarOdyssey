using SolarOdyssey.UI;
using UnityEngine;

namespace SolarOdyssey
{
    public class GoldPickup : MonoBehaviour
    {
        [SerializeField] private int goldAmount = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            CoinUI coinUI = FindFirstObjectByType<CoinUI>();

            if (coinUI != null)
            {
                coinUI.AddGold(goldAmount);
            }

            // Altın sesi
            if (Audio.EnvironmentAudio.Instance != null)
            {
                Audio.EnvironmentAudio.Instance.PlayGold();
            }

            Debug.Log("Altın alındı: +" + goldAmount);

            Destroy(gameObject);
        }
    }
}