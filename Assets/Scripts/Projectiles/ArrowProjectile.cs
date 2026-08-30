using UnityEngine;
using SolarOdyssey.Combat;

namespace SolarOdyssey.Projectiles
{
    public class ArrowProjectile : MonoBehaviour
    {
        [Header("Arrow Settings")]
        [SerializeField] private float speed = 12f;
        [SerializeField] private int damage = 10;
        [SerializeField] private float lifetime = 5f;

        private Vector2 direction;
        private bool launched;

        private void Awake()
        {
            SpriteRenderer[] renderers =
                GetComponentsInChildren<SpriteRenderer>(true);

            Debug.Log(
                "Arrow SpriteRenderer sayısı: " +
                renderers.Length
            );

            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
                renderer.sortingOrder = 100;

                Debug.Log(
                    "Arrow Sprite: " +
                    renderer.sprite +
                    " | Enabled: " +
                    renderer.enabled +
                    " | SortingOrder: " +
                    renderer.sortingOrder
                );
            }
        }

        public void Launch(Vector2 newDirection)
        {
            direction =
                newDirection.normalized;

            launched = true;

            // Ok uçuş yönüne dönsün.
            float angle =
                Mathf.Atan2(
                    direction.y,
                    direction.x
                ) * Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    angle
                );

            Debug.Log(
                "Arrow Launch edildi. " +
                "Pozisyon: " +
                transform.position +
                " | Yön: " +
                direction
            );

            // Belirli süre sonra yok ol.
            Destroy(
                gameObject,
                lifetime
            );
        }

        private void Update()
        {
            if (!launched)
                return;

            transform.position +=
                (Vector3)(
                    direction *
                    speed *
                    Time.deltaTime
                );
        }

        private void OnTriggerEnter2D(
            Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Health health =
                other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(
                    damage
                );

                Debug.Log(
                    "Enemy3 oku oyuncuya " +
                    damage +
                    " hasar verdi!"
                );
            }

            Destroy(gameObject);
        }
    }
}