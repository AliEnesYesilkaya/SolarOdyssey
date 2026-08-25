using UnityEngine;
using UnityEngine.UI;
using SolarOdyssey.Combat;

namespace SolarOdyssey.Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBarFill;
        [SerializeField] private float visibleTime = 2f;

        private Health health;
        private float hideTimer;
        private int lastHealth;

        private void Awake()
        {
            health = GetComponentInParent<Health>();

            if (health == null)
            {
                Debug.LogError("EnemyHealthBar: Health bulunamadı!");
                return;
            }

            lastHealth = health.CurrentHealth;

            healthBarFill.fillAmount = 1f;
            healthBarFill.enabled = false;
        }

        private void Update()
        {
            if (health == null)
                return;

            int currentHealth = health.CurrentHealth;

            if (currentHealth < lastHealth)
            {
                healthBarFill.enabled = true;
                hideTimer = visibleTime;
            }

            lastHealth = currentHealth;

            healthBarFill.fillAmount =
                (float)currentHealth / health.MaxHealth;

            if (healthBarFill.enabled)
            {
                hideTimer -= Time.deltaTime;

                if (hideTimer <= 0f)
                {
                    healthBarFill.enabled = false;
                }
            }
        }
    }
}