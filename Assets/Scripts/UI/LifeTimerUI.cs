using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class LifeTimerUI : MonoBehaviour
    {
        [SerializeField] private Image lifeHeart;
        [SerializeField] private TMP_Text timerText;

        private void Awake()
        {
            // Başlangıçta 5 kalp olduğu için
            // timer kutusu kapalı.
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateTimer(float remainingTime)
        {
            int minutes =
                Mathf.FloorToInt(
                    remainingTime / 60f
                );

            int seconds =
                Mathf.FloorToInt(
                    remainingTime % 60f
                );

            if (timerText != null)
            {
                timerText.text =
                    minutes.ToString("00") +
                    ":" +
                    seconds.ToString("00");
            }
        }
    }
}