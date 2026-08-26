using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class LifeUI : MonoBehaviour
    {
        [Header("Heart Images")]
        [SerializeField] private Image[] hearts;

        public void UpdateLives(
            int currentLives,
            int maxLives)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] == null)
                    continue;

                // Oyuncunun sahip olduğu kalpler görünür.
                // Örneğin 4 kalp varsa:
                // Heart1-4 açık, Heart5 kapalı.
                hearts[i].enabled =
                    i < currentLives;
            }
        }
    }
}