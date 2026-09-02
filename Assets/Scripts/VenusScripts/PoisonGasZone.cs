using TMPro;
using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Environment
{
    public class PoisonGasZone : MonoBehaviour
    {
        [Header("Poison Damage")]
        [SerializeField] private float damageInterval = 2f;
        [SerializeField] private int poisonDamage = 3;

        [Header("Warning UI")]
        [SerializeField] private TMP_Text warningText;
        [SerializeField] private float blinkSpeed = 5f;

        private Health playerHealth;

        private float damageTimer;

        private bool playerInside;

        private void Start()
        {
            if (warningText != null)
            {
                warningText.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!playerInside || playerHealth == null)
                return;

            // Zehir hasarı
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;

                playerHealth.TakeDamage(poisonDamage);
            }

            // Uyarı yazısını yanıp söndür
            if (warningText != null)
            {
                float alpha =
                    Mathf.PingPong(
                        Time.time * blinkSpeed,
                        1f
                    );

                Color textColor =
                    warningText.color;

                textColor.a = alpha;

                warningText.color =
                    textColor;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            playerHealth =
                other.GetComponent<Health>();

            if (playerHealth == null)
                return;

            playerInside = true;

            damageTimer = 0f;

            if (warningText != null)
            {
                warningText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            playerInside = false;

            playerHealth = null;

            damageTimer = 0f;

            if (warningText != null)
            {
                warningText.gameObject.SetActive(false);
            }
        }
    }
}