using SolarOdyssey.Combat;
using SolarOdyssey.UI;
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
            startPosition =
                transform.position;

            startRotation =
                transform.rotation;
        }

        private void OnTriggerEnter2D(
            Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Health health =
                other.GetComponent<Health>();

            if (health == null)
                return;

            if (health.IsFullHealth())
                return;

            // --------------------------------------------
            // HEART EFFECT BUL
            // --------------------------------------------

            HeartCollectEffect effect =
                FindFirstObjectByType<HeartCollectEffect>(
                    FindObjectsInactive.Include
                );

            // --------------------------------------------
            // CANVAS BUL
            // --------------------------------------------

            Canvas canvas =
                FindFirstObjectByType<Canvas>(
                    FindObjectsInactive.Include
                );

            // --------------------------------------------
            // HEALTH BAR BUL
            // --------------------------------------------

            PlayerHealthBar healthBar =
                FindFirstObjectByType<PlayerHealthBar>(
                    FindObjectsInactive.Include
                );

            RectTransform healthBarTarget = null;

            if (healthBar != null)
            {
                healthBarTarget =
                    healthBar.GetComponent<RectTransform>();
            }

            // --------------------------------------------
            // KALP SPRITE'INI AL
            // --------------------------------------------

            SpriteRenderer spriteRenderer =
                GetComponent<SpriteRenderer>();

            Sprite heartSprite = null;

            if (spriteRenderer != null)
            {
                heartSprite =
                    spriteRenderer.sprite;
            }

            Debug.Log(
                "Heart Effect kontrolü: " +
                "Effect=" + (effect != null) +
                " Canvas=" + (canvas != null) +
                " HealthBar=" + (healthBarTarget != null) +
                " Sprite=" + (heartSprite != null)
            );

            // --------------------------------------------
            // EFEKTİ BAŞLAT
            // --------------------------------------------

            if (effect != null &&
                canvas != null &&
                healthBarTarget != null &&
                heartSprite != null)
            {
                effect.Setup(
                    canvas,
                    healthBarTarget,
                    heartSprite,
                    transform.position,
                    healAmount,
                    health
                );
            }
            else
            {
                // Bir şey eksikse oyuncunun canı yine dolsun.
                health.Heal(healAmount);

                Debug.LogWarning(
                    "Heart Effect kurulamadı. " +
                    "Can direkt verildi."
                );
            }

            // Kalp sesi.
            if (Audio.EnvironmentAudio.Instance != null)
            {
                Audio.EnvironmentAudio.Instance.PlayHeart();
            }

            Debug.Log(
                "Kalp alındı: +" +
                healAmount
            );

            // Pickup'ı kapat.
            gameObject.SetActive(false);
        }

        public void Respawn()
        {
            transform.position =
                startPosition;

            transform.rotation =
                startRotation;

            gameObject.SetActive(true);
        }
    }
}