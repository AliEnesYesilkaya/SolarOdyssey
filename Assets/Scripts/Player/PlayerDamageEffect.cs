using System.Collections;
using UnityEngine;

namespace SolarOdyssey.Player
{
    public class PlayerDamageEffect : MonoBehaviour
    {
        [Header("Damage Effect")]
        [SerializeField] private float flashDuration = 0.1f;

        [Header("Damage Freeze")]
        [SerializeField] private float freezeDuration = 0.2f;

        private SpriteRenderer[] spriteRenderers;

        private Rigidbody2D rb;
        private PlayerMovement playerMovement;
        private PlayerController playerController;

        private Coroutine damageCoroutine;

        private void Awake()
        {
            // Player'ın kendisinde ve alt objelerinde
            // bulunan SpriteRenderer'ları bulur.
            spriteRenderers =
                GetComponentsInChildren<SpriteRenderer>();

            rb =
                GetComponent<Rigidbody2D>();

            playerMovement =
                GetComponent<PlayerMovement>();

            playerController =
                GetComponent<PlayerController>();
        }

        public void PlayDamageEffect()
        {
            // Önceki hasar efekti devam ediyorsa
            // onu durdur.
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }

            damageCoroutine =
                StartCoroutine(DamageEffect());
        }

        private IEnumerator DamageEffect()
        {
            // -----------------------------------------
            // HAREKETİ DURDUR
            // -----------------------------------------

            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // Mevcut hareket hızını sıfırla.
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }

            // -----------------------------------------
            // OYUNCUYU KIRMIZI YAP
            // -----------------------------------------

            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                if (sprite != null)
                {
                    sprite.color = Color.red;
                }
            }

            // Hasar sırasında kısa süre bekle.
            yield return new WaitForSeconds(
                freezeDuration
            );

            // -----------------------------------------
            // RENGİ NORMALE DÖNDÜR
            // -----------------------------------------

            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                if (sprite != null)
                {
                    sprite.color = Color.white;
                }
            }

            // -----------------------------------------
            // HAREKETİ GERİ AÇ
            // -----------------------------------------

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            if (playerController != null)
            {
                playerController.enabled = true;
            }

            damageCoroutine = null;
        }
    }
}