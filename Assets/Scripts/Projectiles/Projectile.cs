using UnityEngine;
using SolarOdyssey.Combat;

namespace SolarOdyssey.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData projectileData;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 direction)//mermiyi fırlatan fonksiyon
        {
            rb.linearVelocity =
                direction.normalized * projectileData.Speed;//yöne göre fiziksel hız

            Destroy(//süre sonu sil
                gameObject,
                projectileData.Lifetime
            );
        }

        private void OnTriggerEnter2D(Collider2D collision)//tag kontrol çarpışma 
        {
            if (!collision.CompareTag("Player"))
                return;

            Health health = collision.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(projectileData.Damage);
            }

            Destroy(gameObject);//mermi çarpışınca kendini yok eder 
        }
    }
}