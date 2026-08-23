using SolarOdyssey.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private Image bossBarImage;
        [SerializeField] private Health bossHealth;

        [SerializeField] private Sprite[] healthSprites;

        private void Start()
        {
            UpdateBar();
        }

        private void Update()
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            if (bossBarImage == null || bossHealth == null)
                return;

            if (healthSprites == null || healthSprites.Length == 0)
                return;

            float healthPercent =
                (float)bossHealth.CurrentHealth /
                bossHealth.MaxHealth;

            healthPercent = Mathf.Clamp01(healthPercent);

            int index = Mathf.RoundToInt(
                healthPercent * (healthSprites.Length - 1)
            );

            bossBarImage.sprite = healthSprites[index];
        }
    }
}