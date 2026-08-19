using UnityEngine;
using SolarOdyssey.Projectiles;

namespace SolarOdyssey.Enemy
{
    public class Enemy3 : EnemyBase
    {
        [Header("Ranged Enemy Settings")]
        [SerializeField] private float attackCooldown = 1.5f;//saldırı arası süre
        [SerializeField] private float verticalTolerance = 1f;//y eksen fark

        [Header("Arrow Settings")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform arrowPoint;

        private float attackTimer;

        protected override void Awake()
        {
            base.Awake();

            attackTimer = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;

            // Oyuncuyla yatay mesafe
            float horizontalDistance = Mathf.Abs(
                player.position.x - transform.position.x
            );

            // Oyuncuyla dikey mesafe
            float verticalDistance = Mathf.Abs(
                player.position.y - transform.position.y
            );

            // Oyuncu algılama alanı dışındaysa Idle
            if (horizontalDistance > detectionRange)
            {
                animator.SetFloat("Speed", 0f);
                return;
            }

            // Oyuncuyu gördüğü anda ona dön
            FacePlayer();

            // Enemy3 yerinde durur
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            // Şimdilik Walk yok, yerinde duruyor
            animator.SetFloat("Speed", 0f);

            // Y hizası uygun değilse saldırma
            if (verticalDistance > verticalTolerance)
                return;

            // Saldırı cooldown
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Attack();

                attackTimer = attackCooldown;
            }
        }

        private void Attack()
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Debug.Log("Enemy3 OK ATIYOR!");
        }

        // Animation Event tarafından çağrılacak
        public void FireArrow()
        {
            if (arrowPrefab == null)
            {
                Debug.LogWarning("Enemy3: Arrow Prefab atanmadı!");
                return;
            }

            if (arrowPoint == null)
            {
                Debug.LogWarning("Enemy3: Arrow Point atanmadı!");
                return;
            }

            if (player == null)
                return;

            // Oku ArrowPoint'te oluştur
            GameObject arrow = Instantiate(
                arrowPrefab,
                arrowPoint.position,
                Quaternion.identity
            );

            // Oyuncuya doğru yön
            Vector2 direction = (
                player.position - arrowPoint.position
            ).normalized;

            // Arrow üzerindeki uçuş scriptini bul
            ArrowProjectile projectile =
                arrow.GetComponent<ArrowProjectile>();

            if (projectile != null)
            {
                projectile.Launch(direction);
            }
        }
    }
}