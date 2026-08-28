using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class GoldCollectEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] private float flyDuration = 0.5f;
        [SerializeField] private float startScale = 0.7f;
        [SerializeField] private float endScale = 0.4f;

        public void Setup(
            RectTransform targetRect,
            Sprite goldSprite,
            Vector3 startWorldPosition,
            int goldAmount,
            CoinUI coinUI)
        {
            if (targetRect == null)
            {
                Debug.LogError(
                    "GoldCollectEffect: CoinIcon hedefi bulunamadı!"
                );
                return;
            }

            if (goldSprite == null)
            {
                Debug.LogError(
                    "GoldCollectEffect: Gold Sprite bulunamadı!"
                );
                return;
            }

            Canvas canvas =
                targetRect.GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                Debug.LogError(
                    "GoldCollectEffect: CoinIcon bir Canvas altında değil!"
                );
                return;
            }

            // --------------------------------------------
            // UÇAN COIN OLUŞTUR
            // --------------------------------------------

            GameObject iconObject =
                new GameObject("FlyingGold");

            iconObject.transform.SetParent(
                canvas.transform,
                false
            );

            Image icon =
                iconObject.AddComponent<Image>();

            icon.sprite =
                goldSprite;

            icon.preserveAspect =
                true;

            RectTransform iconRect =
                icon.GetComponent<RectTransform>();

            // --------------------------------------------
            // BAŞLANGIÇ KONUMU
            // --------------------------------------------

            Camera mainCamera =
                Camera.main;

            if (mainCamera == null)
            {
                Destroy(iconObject);
                return;
            }

            Vector2 screenPosition =
                mainCamera.WorldToScreenPoint(
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

            iconRect.anchoredPosition =
                localPosition;

            iconRect.localScale =
                Vector3.one * startScale;

            // Uçuşu başlat.
            StartCoroutine(
                FlyToCoin(
                    iconRect,
                    targetRect,
                    goldAmount,
                    coinUI
                )
            );
        }

        private IEnumerator FlyToCoin(
            RectTransform icon,
            RectTransform target,
            int goldAmount,
            CoinUI coinUI)
        {
            Vector3 startPosition =
                icon.position;

            float timer = 0f;

            while (timer < flyDuration)
            {
                if (icon == null ||
                    target == null)
                {
                    yield break;
                }

                timer +=
                    Time.unscaledDeltaTime;

                float t =
                    timer / flyDuration;

                t =
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        t
                    );

                // Hedef her frame yeniden okunuyor.
                // Böylece UI hareket ederse bile takip eder.
                Vector3 targetPosition =
                    target.position;

                icon.position =
                    Vector3.Lerp(
                        startPosition,
                        targetPosition,
                        t
                    );

                icon.localScale =
                    Vector3.Lerp(
                        Vector3.one * startScale,
                        Vector3.one * endScale,
                        t
                    );

                yield return null;
            }

            // CoinIcon'a ulaştı.
            if (coinUI != null)
            {
                coinUI.AddGold(
                    goldAmount
                );
            }

            Destroy(
                icon.gameObject
            );
        }
    }
}