using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SolarOdyssey.Combat;

namespace SolarOdyssey.UI
{
    public class HeartCollectEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] private float flyDuration = 0.5f;
        [SerializeField] private float startScale = 0.7f;
        [SerializeField] private float endScale = 0.4f;

        public void Setup(
            Canvas canvas,
            RectTransform target,
            Sprite heartSprite,
            Vector3 startWorldPosition,
            int healAmount,
            Health playerHealth)
        {
            if (canvas == null)
            {
                Debug.LogError("Heart Effect: Canvas bulunamadı!");
                return;
            }

            if (target == null)
            {
                Debug.LogError("Heart Effect: HealthBar hedefi bulunamadı!");
                return;
            }

            if (heartSprite == null)
            {
                Debug.LogError("Heart Effect: Kalp Sprite bulunamadı!");
                return;
            }

            Debug.Log("Heart Effect BAŞLADI!");

            // Uçacak kalp objesini oluştur.
            GameObject flyingHeart =
                new GameObject("FlyingHeart");

            flyingHeart.transform.SetParent(
                canvas.transform,
                false
            );

            Image image =
                flyingHeart.AddComponent<Image>();

            image.sprite = heartSprite;
            image.preserveAspect = true;
            image.raycastTarget = false;

            RectTransform heartRect =
                flyingHeart.GetComponent<RectTransform>();

            heartRect.sizeDelta =
                new Vector2(60f, 60f);

            // Dünya konumunu ekran konumuna çevir.
            Camera cam = Camera.main;

            if (cam == null)
            {
                Debug.LogError(
                    "Heart Effect: Main Camera bulunamadı!"
                );

                Destroy(flyingHeart);
                return;
            }

            Vector2 screenPosition =
                cam.WorldToScreenPoint(
                    startWorldPosition
                );

            RectTransform canvasRect =
                canvas.GetComponent<RectTransform>();

            Camera canvasCamera =
                canvas.renderMode ==
                RenderMode.ScreenSpaceOverlay
                    ? null
                    : canvas.worldCamera;

            RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPosition,
                    canvasCamera,
                    out Vector2 localPosition
                );

            heartRect.anchoredPosition =
                localPosition;

            heartRect.localScale =
                Vector3.one * startScale;

            StartCoroutine(
                FlyToHealthBar(
                    heartRect,
                    target,
                    healAmount,
                    playerHealth
                )
            );
        }

        private IEnumerator FlyToHealthBar(
            RectTransform heart,
            RectTransform target,
            int healAmount,
            Health playerHealth)
        {
            Vector3 startPosition =
                heart.position;

            Vector3 targetPosition =
                target.position;

            float timer = 0f;

            while (timer < flyDuration)
            {
                if (heart == null)
                    yield break;

                timer +=
                    Time.unscaledDeltaTime;

                float t =
                    Mathf.Clamp01(
                        timer / flyDuration
                    );

                t =
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        t
                    );

                heart.position =
                    Vector3.Lerp(
                        startPosition,
                        targetPosition,
                        t
                    );

                heart.localScale =
                    Vector3.Lerp(
                        Vector3.one * startScale,
                        Vector3.one * endScale,
                        t
                    );

                yield return null;
            }

            // Kalp bara ulaştığında iyileştir.
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            Debug.Log(
                "Heart Effect tamamlandı. +" +
                healAmount +
                " can."
            );

            Destroy(heart.gameObject);
        }
    }
}