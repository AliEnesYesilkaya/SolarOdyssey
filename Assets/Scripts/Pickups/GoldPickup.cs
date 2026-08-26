using SolarOdyssey.UI;
using UnityEngine;

namespace SolarOdyssey
{
    public class GoldPickup : MonoBehaviour
    {
        [SerializeField] private int goldAmount = 10;

        private Vector3 startPosition;
        private Quaternion startRotation;

        private void Awake()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            CoinUI coinUI =
                FindFirstObjectByType<CoinUI>();

            if (coinUI != null)
            {
                coinUI.AddGold(goldAmount);
            }

            if (Audio.EnvironmentAudio.Instance != null)
            {
                Audio.EnvironmentAudio.Instance.PlayGold();
            }

            Debug.Log(
                "Altın alındı: +" +
                goldAmount
            );

            gameObject.SetActive(false);
        }

        public void Respawn()
        {
            transform.position = startPosition;
            transform.rotation = startRotation;

            gameObject.SetActive(true);
        }
    }
}