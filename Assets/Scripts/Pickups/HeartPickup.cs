using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey
{
    public class HeartPickup : MonoBehaviour
    {
        [SerializeField] private int healAmount = 15;

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

            Health health =
                other.GetComponent<Health>();

            if (health == null)
                return;

            if (health.IsFullHealth())
                return;

            health.Heal(healAmount);

            // Kalp toplama sesi
            if (Audio.EnvironmentAudio.Instance != null)
            {
                Audio.EnvironmentAudio.Instance.PlayHeart();
            }

            Debug.Log(
                "Kalp alındı: +" +
                healAmount
            );

            // Destroy etmiyoruz.
            // Respawn sırasında tekrar aktif olacak.
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