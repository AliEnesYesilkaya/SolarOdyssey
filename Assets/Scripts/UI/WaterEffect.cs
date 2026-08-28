using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class WaterEffect : MonoBehaviour
    {
        [SerializeField] private Image redOverlay;

        [Header("Effect")]
        [SerializeField] private float fadeSpeed = 3f;

        private Color overlayColor;

        private bool isInWater;

        private void Awake()
        {
            if (redOverlay == null)
                return;

            overlayColor =
                redOverlay.color;

            overlayColor.a = 0f;

            redOverlay.color =
                overlayColor;
        }

        private void Update()
        {
            if (redOverlay == null)
                return;

            float targetAlpha =
                isInWater ? 0.25f : 0f;

            overlayColor =
                redOverlay.color;

            overlayColor.a =
                Mathf.MoveTowards(
                    overlayColor.a,
                    targetAlpha,
                    fadeSpeed *
                    Time.deltaTime
                );

            redOverlay.color =
                overlayColor;
        }

        public void EnterWater()
        {
            isInWater = true;
        }

        public void ExitWater()
        {
            isInWater = false;
        }
    }
}