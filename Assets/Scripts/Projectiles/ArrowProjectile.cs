using UnityEngine;
using SolarOdyssey.Combat;

namespace SolarOdyssey.Projectiles
{
    public class ArrowProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private int damage = 10;

        private Vector2 direction;

        public void Launch(Vector2 newDirection)
        {
            direction = newDirection.normalized;

            // Ok uçuş yönüne dönsün
            float angle = Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                angle
            );
        }

        private void Update()
        {
            transform.position +=
                (Vector3)(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Health health = other.GetComponent<Health>();

                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                Destroy(gameObject);
            }
        }
    }
}