using SolarOdyssey.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class BossHealthBar : MonoBehaviour
    {
        [Header("Boss Health")]
        [SerializeField] private Image bossBarImage;
        [SerializeField] private Health bossHealth;

        [Header("Boss Detection")]
        [SerializeField] private Transform boss;
        [SerializeField] private float showDistance = 15f;

        [Header("Player")]
        [SerializeField] private Transform player;

        private void Awake()
        {
            // Player'ı otomatik bul.
            if (player == null)
            {
                GameObject playerObject =
                    GameObject.FindGameObjectWithTag("Player");

                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            // Boss'un canını otomatik bul.
            if (bossHealth == null && boss != null)
            {
                bossHealth =
                    boss.GetComponent<Health>();
            }

            // Bar başlangıçta gizli.
            if (bossBarImage != null)
            {
                bossBarImage.enabled = false;
            }
        }

        private void Update()
        {
            if (bossBarImage == null)
                return;

            if (bossHealth == null)
                return;

            if (player == null)
                return;

            if (boss == null)
                return;

            // Oyuncu ile Boss arasındaki mesafe.
            float distance =
                Vector2.Distance(
                    player.position,
                    boss.position
                );

            // Boss'a yeterince yaklaşıldıysa barı göster.
            bool shouldShow =
                distance <= showDistance;

            bossBarImage.enabled = shouldShow;

            if (!shouldShow)
                return;

            UpdateBar();
        }

        private void UpdateBar()
        {
            float healthPercent =
                (float)bossHealth.CurrentHealth /
                bossHealth.MaxHealth;

            healthPercent =
                Mathf.Clamp01(healthPercent);

            bossBarImage.fillAmount =
                healthPercent;
        }
    }
}