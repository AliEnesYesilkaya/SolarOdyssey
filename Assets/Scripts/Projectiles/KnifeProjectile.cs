using SolarOdyssey.Combat;
using SolarOdyssey.Player;
using UnityEngine;

namespace Assets.Scripts.Projectiles
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
            rb =
                GetComponent<Rigidbody2D>();

            Debug.Log(
                "KnifeProjectile Awake çalıştı: " +
                gameObject.name
            );
        }

        public void Launch(
      Vector2 direction,
      PlayerUpgradeSystem upgrades,
      GameObject shooter)
        {
            Debug.Log(
                "KnifeProjectile: Launch() ÇALIŞTI!"
            );

            upgradeSystem = upgrades;
            owner = shooter;

            direction = direction.normalized;

            rb.linearVelocity =
                direction * speed;

            Debug.Log(
                "KnifeProjectile: Velocity = " +
                rb.linearVelocity
            );

            Destroy(
                gameObject,
                lifeTime
            );
        }

        private void OnTriggerEnter2D(
            Collider2D other)
        {
            Debug.Log(
                "Bıçak bir collider'a çarptı: " +
                other.name
            );

            // =========================================
            // KENDİ OYUNCUMUZA ÇARPMASIN
            // =========================================

            if (owner != null &&
                other.transform.IsChildOf(
                    owner.transform
                ))
            {
                Debug.Log(
                    "Bıçak kendi oyuncusuna çarptı."
                );

                return;
            }

            // =========================================
            // HEALTH BUL
            // =========================================

            Health health =
                other.GetComponentInParent<Health>();

            if (health == null)
            {
                Debug.Log(
                    "Çarpılan objede Health bulunamadı: " +
                    other.name
                );

                return;
            }

            // =========================================
            // UPGRADE SYSTEM KONTROLÜ
            // =========================================

            if (upgradeSystem == null)
            {
                Debug.LogError(
                    "KnifeProjectile: " +
                    "PlayerUpgradeSystem bulunamadı!"
                );

                return;
            }

            // =========================================
            // HASAR
            // =========================================

            int damage =
                upgradeSystem.GetKnifeDamage();

            Debug.Log(
                "BIÇAK ENEMY'YE ÇARPTI! " +
                "Hasar: " +
                damage
            );

            health.TakePlayerAttackDamage(
                damage,
                damage
            );

            // Bıçak düşmana çarptıktan sonra yok olur.
            Destroy(gameObject);
        }
    }
}