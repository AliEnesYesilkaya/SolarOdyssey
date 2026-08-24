using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey
{
    public class HeartPickup : MonoBehaviour
    {
        [SerializeField] private int healAmount = 15;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                if (health.IsFullHealth())
                    return;

                health.Heal(healAmount);

                // Kalp toplama sesi
                if (Audio.EnvironmentAudio.Instance != null)
                {
                    Audio.EnvironmentAudio.Instance.PlayHeart();
                }

                Destroy(gameObject);
            }
        }
    }
}