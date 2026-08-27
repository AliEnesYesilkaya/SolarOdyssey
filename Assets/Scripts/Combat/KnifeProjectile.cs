using SolarOdyssey.Combat;
using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.Projectiles
{
    public class KnifeProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 3f;

        private Rigidbody2D rb;
        private PlayerUpgradeSystem upgradeSystem;
        private GameObject owner;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(
            Vector2 direction,
            PlayerUpgradeSystem upgrades,
            GameObject shooter)
        {
            upgradeSystem = upgrades;
            owner = shooter;

            rb.linearVelocity =
                direction.normalized * speed;

            Destroy(
                gameObject,
                lifeTime
            );
        }

        private void OnTriggerEnter2D(
            Collider2D other)
        {
            // Bıçak kendi oyuncusuna çarparsa
            // hasar vermeden devam eder.
            if (owner != null &&
                other.transform.IsChildOf(owner.transform))
            {
                return;
            }

            Health health =
                other.GetComponentInParent<Health>();

            if (health == null)
                return;

            if (upgradeSystem == null)
                return;

            health.TakePlayerAttackDamage(
                upgradeSystem.GetKnifeDamage(),
                upgradeSystem.GetKnifeDamage()
            );

            Destroy(gameObject);
        }
    }
}